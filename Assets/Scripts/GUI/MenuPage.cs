using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameStates
{
    public class MenuPage : PageBasement, IMenuPageable
    {
        [SerializeField] private Button _battle;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _settings;

        private void Start()
        {
            _battle.onClick.AddListener(() => Menu.Instance.SwitchPage<BattlePage>());
            _shop.onClick.AddListener(() => Menu.Instance.SwitchPage<ShopPage>());
            _settings.onClick.AddListener(() => Menu.Instance.SwitchPage<SettingsPage>());
            
            OpenButton();
        }

        public void OpenButton()
        {
            Menu.Instance.SwitchPage<MenuPage>();
        }

        public void SendArgs<T>(T args) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}