using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Behaviours;
using UnityEngine;


namespace Abilities.Core
{
    public enum ActionType
    {
        Use,
        Init,
        GetData
    }

    public class AbilitiesDirector : MonoBehaviour
    {
        [SerializeField] private List<Ability> _data = new List<Ability>();

        private void Awake()
        {
            _data = GetComponents<Ability>().ToList();
        }

        public List<Ability> GetAll()
        {
            return _data;
        }

        public Ability GetAbility(Type t)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].GetType() == t)
                    return _data[i];
            }

            return null;
        }
    }
}
