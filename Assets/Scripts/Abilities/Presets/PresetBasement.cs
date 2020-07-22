using System;
using System.Collections.Generic;
using Abilities.Behaviours;
using Abilities.Core;
using Abilities.UI;
using GameStates;
using UnityEngine;
using Object = System.Object;

namespace Abilities.Presets
{
    public abstract class PresetBasement : MonoBehaviour, IAbilityPresetHandler
    {
        [SerializeField] protected Type[] _objects; 
        
        public virtual void Load(AbilitiesDirector director)
        { 
            InitData data = new InitData()
           {
               Presenter = FindObjectOfType<AbilityPresenter>(),
               Logic = FindObjectOfType<LevelData>()
           };

            for (int i = 0; i < _objects.Length; i++)
            {
                var ability = director.GetAbility(_objects[i]);

                var receiver = ability as IDataReceiver<InitData>;
                Debug.Log(_objects[i]);
                receiver?.Interact(data, Callback);
            }
        }

        protected abstract void Callback(IAbilityData obj);
    }
}
