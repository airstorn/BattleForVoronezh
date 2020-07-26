using System;
using Core;
using UnityEngine;

namespace User
{
    public class Money : MonoBehaviour, IResourcable<int>
    {
        public event Action<int> OnValueChanged;

        private int _money;
        
        public void Add(int obj)
        {
            _money += obj;
            OnValueChanged?.Invoke(_money);
        }

        public int Get()
        {
            return _money;
        }

        public void Remove(int obj)
        {
            _money -= obj;
            OnValueChanged?.Invoke(_money);
        }
    }
}
