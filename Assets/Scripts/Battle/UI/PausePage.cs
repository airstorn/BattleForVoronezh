using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePage : PageBasement, IMenuPagable
{
   public void ExitToMenuButton()
   {
      SceneManager.LoadScene(0);
   }

   public override void Show()
   {
      base.Show();
      Time.timeScale = 0;
   }

   public override void Hide()
   {
      base.Hide();
      Time.timeScale = 1;
   }
}
