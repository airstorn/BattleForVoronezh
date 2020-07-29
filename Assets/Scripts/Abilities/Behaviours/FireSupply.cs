using System;
using System.Linq;
using Abilities.Core;
using Abilities.UI;
using Battle.Interfaces;
using Core;
using GameStates;
using Interfaces;
using UnityEngine;
using User;

namespace Abilities.Behaviours
{
    public class FireSupply : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IDataReceiver<ShopData>, IGradable
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private int _gradePrice;
        [SerializeField] private int _buyPrice;
        private Behaviour _behaviour;
        
        private class Behaviour
        {
            public LevelData Logic;
            public MultipleTargetsTracker Handler;
            public AbilityPresenter Presenter;
            public IPlayerState State;

            public void DoAction(AbilityLevel level)
            {
                State.SetInput(Handler, false);
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
                Save();
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
            if(_level == AbilityLevel.level3)
                return;

            if (UserData.Instance.Money.Get() >= _gradePrice)
            {
                _level =  (AbilityLevel) Mathf.Clamp((int)_level+ 1, (int)AbilityLevel.level1, (int)AbilityLevel.level3);
                UserData.Instance.Money.Remove(_gradePrice);
                Save();
            }
        }

        public int GetGradePrice()
        {
            return _gradePrice;
        }

        public void Interact(ShopData data, Action<IAbilityData> callback = null)
        {
            data.Visual = GetData();
            data.GradePrice = _gradePrice;
            data.BuyPrice = _buyPrice;

            callback?.Invoke(data);
        }
    }
}
