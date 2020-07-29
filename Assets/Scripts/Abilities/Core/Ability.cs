using System;
using Core;
using UnityEngine;

namespace Abilities.Core
{
    public abstract class Ability : MonoBehaviour, IResourcable<int>
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


        public event Action<int> OnValueChanged;
        public void Add(int obj)
        {
            _count += obj;
        }

        public int Get()
        {
            return _count;
        }

        public void Remove(int obj)
        {
            _count -= obj;
        }
    }
}