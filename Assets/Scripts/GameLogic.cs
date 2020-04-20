using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GridObject _playerGrid;
    [SerializeField] private GridObject _enemyGrid;
    
    [Header("Statements")]
    [SerializeField] private GameObject _placementTurnObject;
    [SerializeField] private GameObject _playerTurnObject;
    [SerializeField] private GameObject _enemyTurnObject;
    public ICamMover CameraStatement;
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

        CameraStatement = Camera.main.GetComponent<ICamMover>();
    }

    public void ChangeState(IGameState state)
    {
        _state.Deactivate();
        _state = state;
        _state.Activate();
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