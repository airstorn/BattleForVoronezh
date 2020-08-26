using Abilities.Core;

namespace GUI.Core
{
    public interface IItemBehaviour
    {
        void SetItem(IAbilityData data);
        void Refresh();
    }
}