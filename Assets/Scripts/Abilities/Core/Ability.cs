using System;
using UnityEngine;

namespace Abilities.Core
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected AbilityLevel _level = AbilityLevel.level1;
        [SerializeField] protected int _count;
        [SerializeField] protected AbilityObject _abilityData;
        public AbilityLevel Level => _level;

        public abstract void Interact();

        public virtual VisualData GetData()
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
    }
}