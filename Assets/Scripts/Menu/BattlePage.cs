using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameStates
{
    public class BattlePage : PageBasement, IMenuPagable
    {
        [SerializeField] private LevelObject[] _levelsBehaviour;
        [SerializeField] private GameObject _windowObject;
        [SerializeField] private Menu _menu;
        
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
                _menu.SwitchPage(_windowObject, this);
                _levelWindowable.ShowLevelData(_levelsBehaviour[offset]);
            }
            
        }
    }
}