using System;

namespace Abilities.Core
{
    public interface IDataReceiver<T> : IDataReceiver where T : IAbilityData
    {
        void Interact(T data, Action<IAbilityData> callback = null);
    }

    public interface IDataReceiver
    {
    }
}