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

        protected string _levelPath;
        protected string _countPath;

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

        private void Start()
        { 
            _levelPath = "level_" + this.GetType();
            _countPath = "count_" + this.GetType();
            
            OnValueChanged += SaveHandler;
            
            Load();  
        }

        private void SaveHandler(int obj)
        {
            Save();
        }

        protected void Save()
        {
            PlayerPrefs.SetInt(_countPath, _count);
            PlayerPrefs.SetInt(_levelPath, (int)_level);
        }

        private void Load()
        {
            _count = PlayerPrefs.GetInt(_countPath, 0);
            _level = (AbilityLevel)PlayerPrefs.GetInt(_levelPath, (int) AbilityLevel.level1);
        }


        public event Action<int> OnValueChanged;
        public void Add(int obj)
        {
            _count += obj;
            OnValueChanged?.Invoke(_count);
        }

        public int Get()
        {
            return _count;
        }

        public void Remove(int obj)
        {
            _count -= obj;
            OnValueChanged?.Invoke(_count);
        }
    }
}