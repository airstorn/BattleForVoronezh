using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStates.Menu
{
    public class SettingsPage : MonoBehaviour, IMenuPagable
    {
        [SerializeField] private GameObject _object;


        void Start()
        {

        }

        public void Show()
        {
            _object.SetActive(true);
        }

        public void Hide()
        {
            _object.SetActive(false);
        }
    }
}