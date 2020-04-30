using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameStates
{
    public class MenuPage : PageBasement, IMenuPagable
    {
        [SerializeField] private Button _battle;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _settings;

        [SerializeField] private GameObject _battleObject;
        [SerializeField] private GameObject _shopObject;
        [SerializeField] private GameObject _settingsObject;

        private void Start()
        {
            _battle.onClick.AddListener(() => Menu.Instance.SwitchPage(_battleObject, this));
        }
    }
}