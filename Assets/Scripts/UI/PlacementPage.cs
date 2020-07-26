using System;
using Core;
using GameStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class PlacementPage : PageBasement, IMenuPageable
    {
        [SerializeField] private Button _confirm;
        [SerializeField] private Button _rotate;
        [SerializeField] private Button _randomize;
        [SerializeField] private Button _menuButton;
        [SerializeField] private PausePage _pause;

        public struct PlacementData
        {
            public UnityAction RotateAction;
            public UnityAction RandomizeAction;
            public UnityAction ConfirmAction;
        }
        
        private void Start()
        {
            _menuButton.onClick.AddListener(delegate { _pause.OpenPause(); });
        }

        public void SendArgs<T>(T args) where T : struct
        {
            if (args is PlacementData data)
            {
                _rotate.onClick.AddListener(data.RotateAction);
                _confirm.onClick.AddListener(data.ConfirmAction);
                _randomize.onClick.AddListener(data.RandomizeAction);
            }
        }
    }
}
