using System;
using Abilities.Core;
using Abilities.UI;
using Core;
using InputHandlers;
using UnityEngine;
using User;

namespace Abilities.Behaviours
{
    public class ExtraMoves : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IDataReceiver<ExtraMovesData>,IDataReceiver<ShopData>, IGradable
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private int _movePrice;
        [SerializeField] private int _gradePrice;
        [SerializeField] private int _buyPrice;

        private Behaviour _behaviour;
        private GameObject _button;
        private LimitedShotsHandler _handler;
        private class Behaviour
        {
            public GameObject CanvasObject;

            public void DoAction()
            {
                // CanvasObject.SetActive(true);
                
            }

            public void Cancel()
            {
                CanvasObject.SetActive(false);
            }
        }
        
        public override void Interact()
        {
            // _behaviour.DoAction();
            _handler.AddShots((int)_level);
            _button.SetActive(false);
            Save();
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
            if(_count == 0)
                return;
            
            _button = data.Presenter.CreateAbilityButton(_buttonTemplate, GetData());

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
