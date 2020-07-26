using System;
using Abilities.Core;
using Battle.Interfaces;
using Core;
using GameStates;
using Interfaces;
using States.Artillery;
using UnityEngine;

namespace Abilities.Behaviours
{
    public class Scouting : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IGradable
    {
        [SerializeField] private GameObject _scoutingTemplate;
        
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
            _level++;
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
            }
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
