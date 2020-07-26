using System.Collections;
using System.Collections.Generic;
using Core;
using GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePage : PageBasement, IMenuPageable
{
   [SerializeField] private Menu _main;
   public void ExitToMenuButton()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene(0);
   }

   public void OpenPause()
   {
      _main.OpenPageOverlayed<PausePage>();
   }

   public override void Show()
   {
      base.Show();
      Time.timeScale = 0;
   }

   public void SendArgs<T>(T args) where T : struct
   {
      throw new System.NotImplementedException();
   }

   public override void Hide()
   {
      base.Hide();
      Time.timeScale = 1;
   }
}
