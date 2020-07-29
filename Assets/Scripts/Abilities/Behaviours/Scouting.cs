using System;
using Abilities.Core;
using Battle.Interfaces;
using Core;
using GameStates;
using Interfaces;
using States.Artillery;
using UnityEngine;
using User;

namespace Abilities.Behaviours
{
    public class Scouting : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IDataReceiver<ShopData>, IGradable
    {
        [SerializeField] private GameObject _scoutingTemplate;
        [SerializeField] private int _gradePrice;
        [SerializeField] private int _buyPrice;
        private Behaviour _behaviour;
        
        private class Behaviour
        {
            public LevelData Logic;

            public void DoAction(AbilityLevel level)
            {
                var grid = Logic.EnemyGrid;
                int count = (int)level;

                for (int i = 0; i < grid.Units.Count; i++)
                {
                    if(count == 0)
                        break;
                    
                    if (grid.Units[i].Visual.IsHidden == true && count != 0)
                    {
                        grid.Units[i].Visual.SetHidden(false);
                        count--;
                    }
                }
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

        public void Interact(VisualData data, Action<IAbilityData> callback = null)
        {
            callback?.Invoke(GetData());
        }

        public void Interact(InitData data, Action<IAbilityData> callback = null)
        {
            data.Presenter.CreateAbilityButton(_buttonTemplate, GetData());

            var handler = Instantiate(_scoutingTemplate);
            
            _behaviour = new Behaviour
            {
                Logic = data.Logic,
            };

            callback?.Invoke(GetData());
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
            throw new NotImplementedException();
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
