using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStates.Menu
{
   public class ShopPage : MonoBehaviour, IMenuPagable
   {
      [SerializeField] private GameObject _object;


      public void Show()
      {
         _object.SetActive(true);
      }

      public void Hide()
      {
         _object.SetActive(false);
      }
   }
}