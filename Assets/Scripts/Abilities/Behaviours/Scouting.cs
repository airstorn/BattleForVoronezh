using System;
using Abilities.Core;
using UnityEngine;

namespace Abilities.Behaviours
{
    public class Scouting : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>, IGradable
    {
        private Behaviour _behaviour;
        private class Behaviour
        {
            public LevelData Logic;
            public MultipleTargetsTracker Tracker;

            public void DoAction(AbilityLevel level)
            {
                var grid = Logic.EnemyGrid;

                for (int i = 0; i < Mathf.Clamp((int)level, 0, grid.Units.Count); i++)
                {
                    grid.Units[i].SetHidden(false);
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
            data.Presenter.CreateAbilityButton(GetData());
            

            _behaviour = new Behaviour
            {
                Logic = data.Logic
            };


            callback?.Invoke(GetData());
        }

        public override void Interact()
        {
            _behaviour.DoAction(_level);
        }
    }
}
