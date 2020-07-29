using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStates
{
    public class PageEndWindow : PageBasement, IMenuPageable
    {
        [SerializeField] private Text _stateText;
        [SerializeField] private TMP_Text _gainText;
        
        public struct LevelEndData
        {
            public bool Win;
            public int Money;
        }
        
        public void SendArgs<T>(T args) where T : struct
        {
            if (args is LevelEndData data)
            {
                _stateText.text = data.Win == true ? "Победа!" : "Проигрыш";
                _gainText.text = "+ " + data.Money + " <sprite=0>";
            }
        }

        public void ExitToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
