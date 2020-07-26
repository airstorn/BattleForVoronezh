using System;
using UnityEngine;

namespace Abilities.Core
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected AbilityLevel _level = AbilityLevel.level1;
        [SerializeField] protected int _count;
        [SerializeField] protected AbilityObject _abilityData;
        [SerializeField] protected GameObject _buttonTemplate;
        public AbilityLevel Level => _level;

        public Action OnAbilityUsed;

        public abstract void Interact();
        public abstract void Cancel();

        protected virtual VisualData GetData()
        {
            VisualData visual = new VisualData()
            {
                Level = _level,
                Count = _count,
                Name = _abilityData.Name,
                Reference = this
            };

            return visual;
        }

        protected virtual string GetLevelString(AbilityLevel lvl)
        {
            switch (lvl)
            {
                case AbilityLevel.level1:
                    return "ур. 1";
                case AbilityLevel.level2:
                    return "ур. 2";
                case AbilityLevel.level3:
                    return "ур. 3";
                
                default:
                    return "Undifiened";
            }
        }
    }
}