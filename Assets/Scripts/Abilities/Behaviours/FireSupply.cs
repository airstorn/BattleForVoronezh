using System;
using System.Linq;
using Abilities.Core;
using Abilities.UI;
using Battle.Interfaces;
using Core;
using GameStates;
using Interfaces;
using UnityEngine;

namespace Abilities.Behaviours
{
    public class FireSupply : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IGradable
    {
        [SerializeField] private GameObject _template;
        private Behaviour _behaviour;
        
        private class Behaviour
        {
            public LevelData Logic;
            public MultipleTargetsTracker Handler;
            public AbilityPresenter Presenter;
            public IPlayerState State;

            public void DoAction(AbilityLevel level)
            {
                State.SetInput(Handler);
                Presenter.SetVisible(false);
                var grid = Logic.EnemyGrid;

                Handler.SetShotsCount((int)level);
                
                Handler.OnInputStoppedHandler += Animate;
            }

            private void Animate()
            {
                Handler.StartCoroutine(Handler.ShotAnimation(Cancel));
            }

            public void Cancel()
            {
                State.ResetInput();
                Presenter.SetVisible(true);
                Handler.OnInputStoppedHandler -= Animate;
            }
        }
        
        public override void Interact()
        {
            if (_count > 0)
            {
                _behaviour.DoAction(_level);
                _count--;
            }
        }

        public override void Cancel()
        {
            _behaviour.Cancel();
        }

        public void Interact(VisualData data, Action<IAbilityData> callback = null)
        {
            callback?.Invoke(GetData());
        }

        public void Interact(InitData data, Action<IAbilityData> callback = null)
        {
            data.Presenter.CreateAbilityButton(_buttonTemplate, GetData());

            var obj = Instantiate(_template);
            
            _behaviour = new Behaviour
            {
                Handler = obj.GetComponent<MultipleTargetsTracker>(), 
                State = LevelData.Instance.PlayerState,
                Logic = LevelData.Instance,
                Presenter = data.Presenter
            };

            obj.GetComponent<IInitiatable<IPlayerState>>().Init(LevelData.Instance.PlayerState);

            callback?.Invoke(GetData());
        }

        public void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
