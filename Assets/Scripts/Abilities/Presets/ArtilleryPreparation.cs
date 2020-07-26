using System;
using Abilities.Behaviours;
using Abilities.Core;
using UnityEngine;

namespace Abilities.Presets
{
    public class ArtilleryPreparation : PresetBasement
    {
        private void Awake()
        {
            _objects = new[] {typeof(Scouting), typeof(FireSupply), typeof(ExtraMoves)};
        }

        protected override void Callback(IAbilityData obj)
        {
        }
    }
}