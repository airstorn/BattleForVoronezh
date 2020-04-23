using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameStates.Menu
{
    public class BattlePage : PageBasement, IMenuPagable
    {
        [SerializeField] private LevelObject[] _levelsBehaviour;
        [SerializeField] private GameObject _windowObject;
        [SerializeField] private global::Menu _menu;
        
        private ILevelWindowable _levelWindowable;


        private void Start()
        {
            _levelWindowable = _windowObject.GetComponent<ILevelWindowable>();
        }

        public void OpenLevel(int levelId)
        {
            int offset = levelId - 1;
            if (_levelsBehaviour.Length > offset)
            {
                _menu.SwitchPage(_windowObject);
                _levelWindowable.ShowLevelData(_levelsBehaviour[offset]);
            }
            
        }
    }
}