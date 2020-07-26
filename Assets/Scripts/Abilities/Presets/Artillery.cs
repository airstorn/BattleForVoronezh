using System;
using Abilities.Behaviours;
using Abilities.Core;
using Abilities.UI;
using GameStates;
using UnityEngine;

namespace Abilities.Presets
{
    public class Artillery : PresetBasement
    {
        private void Awake()
        {
            _objects = new[] {typeof(Scouting), typeof(FireSupply)};
        }

        protected override void Callback(IAbilityData obj)
        {
        }
    }
}
