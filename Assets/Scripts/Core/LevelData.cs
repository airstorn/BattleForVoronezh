using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using Core.LevelInitiation;
using GameStates;
using UnityEngine;
using User;
using Random = UnityEngine.Random;

namespace Core
{
    public enum LevelType
    {
        ArtPreparation,
        Artillery,
        Tank
    }

    public class LevelData : MonoBehaviour
    {
        [SerializeField] private LevelType _levelType;
    
        [SerializeField] private GridObject _playerGrid;
        [SerializeField] private GridObject _enemyGrid;
    
        [Header("Statements")]
    
        public ICamMover CameraStatement;
        public Action OnUpdate;

        [Header("UI")] 
        [SerializeField] private GameObject _endWindow;

        public GridObject PlayerGrid => _playerGrid; 
        public GridObject EnemyGrid => _enemyGrid;

        public IPlayerState PlayerState => GetState<IPlayerState>();

        private T GetState<T>()
        {
            return FindObjectsOfType<MonoBehaviour>().OfType<T>().First();
        }

        public Action OnPlayerWin;
        public Action OnPlayerLoose;

        private IGameState _state;

        private IGameState[] _cachedStates;

        public static LevelData Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            OnPlayerLoose += PlayerLoose;
            OnPlayerWin += PlayerWin;
                
            ChangeState(FindObjectOfType<PlaceUnits>());
        
            CameraStatement = Camera.main.GetComponent<ICamMover>();
            _cachedStates = FindObjectsOfType<MonoBehaviour>().OfType<IGameState>().ToArray();
        }

     

        public void ChangeState(IGameState state)
        {
            _state?.Deactivate();
            _state = state;
            _state.Activate();
        }

        public void ChangeState<T>() where T : IGameState
        {
            _state?.Deactivate();
            _state = _cachedStates.OfType<T>().First();
            _state.Activate();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void PlayerWin()
        {
            Debug.Log("Player win!");
            var page =  Menu.Instance.SwitchPage<PageEndWindow>();
            
            var data = new PageEndWindow.LevelEndData()
            { 
                Money = Random.Range(400, 550),
                Win = true
            };
            
            UserData.Instance.Money.Add(data.Money);
            page.SendArgs(data);
        }

        private void PlayerLoose()
        {
            Debug.Log("Player loose!");
            var page = Menu.Instance.SwitchPage<PageEndWindow>();
            page.SendArgs(new PageEndWindow.LevelEndData(){ Money = 0, Win = false});
        }
    }
}