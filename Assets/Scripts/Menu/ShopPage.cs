using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Behaviours;
using Abilities.Core;
using Core;
using UnityEngine;
using User;
using Object = UnityEngine.Object;

namespace GameStates
{
   public class ShopPage : PageBasement, IMenuPageable
   {
      [SerializeField] private GameObject _itemTemplate;
      [SerializeField] private IEnumerable<Type> _abilities;
      [SerializeField] private Transform _itemsParent;
      
      private AbilitiesDirector _director;

      private void Start()
      {
         _director = UserData.Instance.AbilitiesDirector;

         var abilities = _director.GetAll();
         for (int i = 0; i < abilities.Count; i++)
         {
            if (abilities[i] is IDataReceiver<VisualData> dataReceiver)
            {
               dataReceiver.Interact(new VisualData(), CreateButton);
            }
         }
      }

      private void CreateButton(IAbilityData data)
      {
         VisualData converted = (VisualData)data;
         
         var obj = Instantiate(_itemTemplate, _itemsParent);

         obj.GetComponent<ShopItem>().SetItem(converted.Name, "" + converted.Level);
      }

      public void SendArgs<T>(T args) where T : struct
      {
         throw new NotImplementedException();
      }
   }
}