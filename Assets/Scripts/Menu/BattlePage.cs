using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameStates.Menu
{
    public class BattlePage : MonoBehaviour, IMenuPagable
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private LevelObject[] _levelsBehaviour;
        [SerializeField] private GameObject _windowObject;
        [SerializeField] private MainMenu _menu;
        
        private ILevelWindowable _levelWindowable;


        private void Start()
        {
            _levelWindowable = _windowObject.GetComponent<ILevelWindowable>();
        }

        public void Show()
        {
            _object.SetActive(true);
        }

        public void Hide()
        {
            _object.SetActive(false);
        }

        public void OpenLevel(int levelId)
        {
            int offset = levelId - 1;
            if (_levelsBehaviour.Length > offset)
            {
                _menu.OpenPage(_windowObject);
                _levelWindowable.ShowLevelData(_levelsBehaviour[offset]);
            }
            
        }
    }
}