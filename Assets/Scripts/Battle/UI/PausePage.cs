using System.Collections;
using System.Collections.Generic;
using GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePage : PageBasement, IMenuPagable
{
   [SerializeField] private Menu _main;
   public void ExitToMenuButton()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene(0);
   }

   public void OpenPause()
   {
      _main.OpenPageOverlayed(gameObject, this);
   }

   public override void Show<T>(T args)
   {
      base.Show(args);
      Time.timeScale = 0;
   }

   public override void Hide()
   {
      base.Hide();
      Time.timeScale = 1;
   }
}
