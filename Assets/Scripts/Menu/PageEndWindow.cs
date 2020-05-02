using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStates
{
    public class PageEndWindow : PageBasement, IMenuPagable
    {
        [SerializeField] private Text _stateText;
        
        public override void Show<T>(T args)
        {
            base.Show(args);
            if (args is bool win)
            {
                _stateText.text = win == true ? "Победа!" : "Проигрыш";
            }
        }

        public void ExitToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
