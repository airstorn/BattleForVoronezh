using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UI;
using UnityEngine;

namespace GameStates
{
   public class Menu : MonoBehaviour
   {

      private IMenuPageable[] _pages;
      private IMenuPageable _currentPage;

      public static Menu Instance;

      private void Awake()
      {
         Instance = this;
         Fill();

         DisableAll();
      }

      private void DisableAll()
      {
         foreach (var page in _pages)
         {
            page.Hide();
         }
      }

      private void Fill()
      {
         var objects = FindObjectsOfType<PageBasement>().OfType<IMenuPageable>();
         _pages = objects.ToArray();
      }

      public IMenuPageable SwitchPage<T>() where T : IMenuPageable
      {
         var pageElement = GetPage<T>();
         
         _currentPage?.Hide();
         _currentPage = pageElement;
         _currentPage.Show();

         return _currentPage;
      } 
      // public void SwitchPage<T>() where T : 
      // {
      //    var pageElement = GetPage(page);
      //    foreach (var tempPage in _pages)
      //    {
      //       if (tempPage == pageElement)
      //       {
      //          tempPage.Show(this);
      //       }
      //       else
      //       {
      //          tempPage.Hide();
      //       }
      //    }
      // }

      public void OpenPageOverlayed<T>() where T : IMenuPageable
      {
         GetPage<T>().Show();
      }

      private IMenuPageable GetPage<T>() where T : IMenuPageable
      {
         return _pages.OfType<T>().First();
      }
   }
}
