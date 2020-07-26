using System;
using Abilities.Behaviours;
using Abilities.Core;
using UnityEngine;

namespace Abilities.Presets
{
    public class TankAttack : PresetBasement
    {
        private void Awake()
        {
            _objects = new[] {typeof(ExtraMoves)};
        }

        protected override void Callback(IAbilityData obj)
        {
        }
    }
}