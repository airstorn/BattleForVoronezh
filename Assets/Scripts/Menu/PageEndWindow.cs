using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStates
{
    public class PageEndWindow : PageBasement, IMenuPagable
    {
        [SerializeField] private Text _stateText;
        [SerializeField] private TMP_Text _gainText;
        
        public override void Show<T>(T args)
        {
            base.Show(args);
            if (args is bool win)
            {
                _stateText.text = win == true ? "Победа!" : "Проигрыш";
                _gainText.text = "+ " + Random.Range(400, 520) + " <sprite=0>";
            }
        }

        public void ExitToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
