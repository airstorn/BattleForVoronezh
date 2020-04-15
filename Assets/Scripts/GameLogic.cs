using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GridObject _playerGrid;
    [SerializeField] private GridObject _enemyGrid;
    [SerializeField] private CameraTurns _cameraStatement;
    
    [Header("Statements")]
    [SerializeField] private GameObject _placementTurnObject;
    [SerializeField] private GameObject _playerTurnObject;
    [SerializeField] private GameObject _enemyTurnObject;
    public CameraTurns CameraStatement => _cameraStatement;
    public Action OnUpdate;

    public GridObject PlayerGrid => _playerGrid; 
    public GridObject EnemyGrid => _enemyGrid;

    private IGameState _state_PlayerTurn;
    private IGameState _state_EnemyTurn;
    private IGameState _state_PlaceUnits;

    private IGameState _state;

    private void Start()
    {
        _state_PlaceUnits = _placementTurnObject.GetComponent<IGameState>();
        _state_PlayerTurn = _playerTurnObject.GetComponent<IGameState>();
        _state_EnemyTurn = _enemyTurnObject.GetComponent<IGameState>();
        
        _state = _state_PlaceUnits;
        _state.Activate();
    }

    public void ChangeState(IGameState state)
    {
        _state.Deactivate();
        _state = state;
        _state.Activate();
        Debug.Log(_state);
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public void Confirm_button()
    {
        // _state.Confirm();
    }
}