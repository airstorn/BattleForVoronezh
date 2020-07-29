using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.Behaviours;
using Abilities.Core;
using Core;
using GUI.Core;
using GUI.Shop;
using UnityEngine;
using User;
using Object = UnityEngine.Object;

namespace GameStates
{
   public class ShopPage : PageBasement, IMenuPageable
   {
      [SerializeField] private GameObject _gradableItemTemplate;
      [SerializeField] private GameObject _consumableItemTemplate;
      [SerializeField] private IEnumerable<Type> _abilities;
      [SerializeField] private Transform _upgradesContainer;
      [SerializeField] private Transform _consumablesContainer;
      
      private AbilitiesDirector _director;

      private void Start()
      {
         _director = UserData.Instance.AbilitiesDirector;

         FillGradables();
         FillConsumables();
      }

      private void FillConsumables()
      {
         Ability[] abilities = new []
         {
            _director.GetAbility(typeof(Scouting)), 
            _director.GetAbility(typeof(FireSupply)) 
         };

         for (int i = 0; i < abilities.Length; i++)
         {
            if (abilities[i] is IDataReceiver<ShopData> dataReceiver)
            {
               dataReceiver.Interact(new ShopData(), ConsumableCallback);
            }
         }
      }

      private void ConsumableCallback(IAbilityData obj)
      {
         CreateButton<ShopConsumableItem>(obj, _consumablesContainer, _consumableItemTemplate);
      }

      private void FillGradables()
      {
         var abilities = _director.GetAll();
         for (int i = 0; i < abilities.Count; i++)
         {
            if (abilities[i] is IDataReceiver<ShopData> dataReceiver )
            {
               if(abilities[i] is IGradable)
               {
                  dataReceiver.Interact(new ShopData(), GradableCallback);

               }
            }
         }
      }

      private void GradableCallback(IAbilityData obj)
      {
         CreateButton<ShopGradableItem>(obj, _upgradesContainer, _gradableItemTemplate);
      }

      private void CreateButton<T>(IAbilityData data, Transform parent, GameObject button) where T : IItemBehaviour
      {
         var obj = Instantiate(button, parent);
         
         obj.GetComponent<T>().SetItem(data);
      }

      public void SendArgs<T>(T args) where T : struct
      {
         throw new NotImplementedException();
      }
      
     
   }
}