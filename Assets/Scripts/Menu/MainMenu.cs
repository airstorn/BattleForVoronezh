using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

   [SerializeField] private GameObject _defaultPage;
   [SerializeField] private GameObject[] _menuObjects;
   private IMenuPagable[] _pages;

   private void Start()
   {
      _pages = _menuObjects.Select(obj => obj.GetComponent<IMenuPagable>()).ToArray();
      OpenPage(_defaultPage);
   }

   public void OpenPage(GameObject page)
   {
      var pageElement = page.GetComponent<IMenuPagable>();
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
}
