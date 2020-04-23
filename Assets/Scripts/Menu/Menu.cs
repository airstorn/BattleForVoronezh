using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Menu : MonoBehaviour
{

   [SerializeField] private GameObject _defaultPage;
   [SerializeField] private GameObject[] _menuObjects;
   private IMenuPagable[] _pages;

   private void Start()
   {
      _pages = _menuObjects.Select(obj => obj.GetComponent<IMenuPagable>()).ToArray();
      SwitchPage(_defaultPage);
   }

   public void SwitchPage(GameObject page)
   {
      var pageElement = GetPage(page);
      foreach (var tempPage in _pages)
      {
         if (tempPage == pageElement)
         {
            tempPage.Show();
         }
         else
         {
            tempPage.Hide();
         }
      }
   }

   public void OpenPageOverlayed(GameObject page)
   {
      GetPage(page).Show();
   }

   private IMenuPagable GetPage(GameObject page)
   {
      return page.GetComponent<IMenuPagable>();
   }
}
