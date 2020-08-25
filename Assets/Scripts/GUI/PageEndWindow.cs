using System.Collections;
using System.Collections.Generic;
using Core;
using Lean.Localization;
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

        private const string _win = "Menu_Win";
        private const string _lose = "Menu_Lose";
        
        public struct LevelEndData
        {
            public bool Win;
            public int Money;
        }
        
        public void SendArgs<T>(T args) where T : struct
        {
            if (args is LevelEndData data)
            {
                _stateText.text = Lean.Localization.LeanLocalization.GetTranslationText(data.Win == true ? _win : _lose);
                _gainText.text = "+ " + data.Money + " <sprite=0>";
            }
        }

        public void ExitToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
