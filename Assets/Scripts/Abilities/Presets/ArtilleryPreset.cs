using Abilities.Behaviours;
using Abilities.Core;
using Abilities.UI;
using GameStates;
using UnityEngine;

namespace Abilities.Presets
{
    public class ArtilleryPreset : MonoBehaviour, IAbilityPresetHandler
    {
        public void Load(AbilitiesDirector director)
        {
            var menu = Menu.Instance;
            InitData data  = new InitData()
            {
                Presenter = FindObjectOfType<AbilityPresenter>()
            };
        }
    }
}
