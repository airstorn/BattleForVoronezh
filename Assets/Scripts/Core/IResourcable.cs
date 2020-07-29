using System;
using UnityEngine.Events;

namespace Core
{
    public interface IResourcable<T> where T : struct
    {
        event Action<T> OnValueChanged;
        void Add(T obj);
        T Get();
        void Remove(T obj);
    }
}
