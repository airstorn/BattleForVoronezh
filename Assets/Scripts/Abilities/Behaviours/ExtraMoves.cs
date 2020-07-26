using System;
using Abilities.Core;
using Abilities.UI;
using Core;
using InputHandlers;
using UnityEngine;
using User;

namespace Abilities.Behaviours
{
    public class ExtraMoves : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IDataReceiver<ExtraMovesData>
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private int _movePrice;

        private Behaviour _behaviour;
        private LimitedShotsHandler _handler;
        private class Behaviour
        {
            public GameObject CanvasObject;

            public void DoAction()
            {
                CanvasObject.SetActive(true);
            }

            public void Cancel()
            {
                CanvasObject.SetActive(false);
            }
        }
        
        public override void Interact()
        {
            _behaviour.DoAction();
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

            _handler = FindObjectOfType<LimitedShotsHandler>();
            
            _behaviour = new Behaviour
            {
                CanvasObject = Instantiate(_template),
            };
            
            _behaviour.CanvasObject.GetComponent<ExtraMovesUi>().Init(_movePrice);
            _behaviour.CanvasObject.SetActive(false);
            callback?.Invoke(GetData());
        }

        public void Interact(ExtraMovesData data, Action<IAbilityData> callback = null)
        {
            var user = UserData.Instance;

            if (user.Money.Get() >= _movePrice)
            {
                _handler.AddShots(1);
                
                user.Money.Remove(_movePrice);
                callback?.Invoke(new ExtraMovesData());
            }
        }
    }
}
