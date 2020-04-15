using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GridObject _playerGrid;
    [SerializeField] private GridObject _enemyGrid;
    [SerializeField] private CameraTurns _cameraStatement;
 
    public CameraTurns CameraStatement => _cameraStatement;

    public GridObject PlayerGrid => _playerGrid;
    public GridObject EnemyGrid => _enemyGrid;

    public GameStates.PlayerTurn _state_PlayerTurn;
    public GameStates.EnemyTurn _state_EnemyTurn;
    public GameStates.PlaceUnits _state_PlaceUnits;

    [Header("Placement Turn")]

    [SerializeField] internal GameObject _PlacementInputHandler;
    [SerializeField] internal GameObject _PlacementUI;

    [Header("Player Turn")]

    [SerializeField] internal GameObject _PlayerTurnInputHandler;

    [Header("Enemy Turn")]

    private GameState _state;


    private void Start()
    {
        _state_PlaceUnits = new GameStates.PlaceUnits(this, _PlacementInputHandler.GetComponent<IInputHandler>());
        _state_PlayerTurn = new GameStates.PlayerTurn(this, _PlayerTurnInputHandler.GetComponent<IInputHandler>());
        _state_EnemyTurn = new GameStates.EnemyTurn(this);

        _state = _state_PlaceUnits;
        _state.Activate();
    }

    private void Update()
    {
        _state.Update();   
    }

    public void ChangeState(GameState state)
    {
        _state.Deactivate();
        _state = state;
        _state.Activate();
        Debug.Log(_state);
    }

    public void Confirm_button()
    {
        _state.Confirm();
    }
}

public class GameState
{
    protected GameLogic _logic;
    protected IInputHandler _playerInputHandler;

    public GameState(GameLogic gameLogic, IInputHandler playerInputHandler)
    {
        _logic = gameLogic;
        _playerInputHandler = playerInputHandler;
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void Update() { }
    public virtual void Confirm() { }
}

namespace GameStates
{
    public class PlaceUnits : GameState
    {
        public PlaceUnits(GameLogic gameLogic, IInputHandler playerInputHandler) : base(gameLogic, playerInputHandler) { }


        public override void Activate()
        {
            _logic._PlacementUI.SetActive(true);
            _logic.StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _logic.CameraStatement.ToPlacerCam();
            yield return null;
            //_logic.ChangeState(_logic._state_PlayerTurn);
        }

        public override void Deactivate()
        {
            _logic._PlacementUI.SetActive(false);
        }

        public override void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _playerInputHandler.TrackInput();
            }
        }

        public override void Confirm()
        {
            if(ValidatePlacement() == true)
            {
                _logic.ChangeState(_logic._state_PlayerTurn);
            }
        }

        private bool ValidatePlacement()
        {

            if (_logic.PlayerGrid.AllUnitsPlaced() == true)
            {
                foreach (var placedElement in _logic.PlayerGrid.Units)
                {
                    if (placedElement.SuitablePlaced == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }

    public class PlayerTurn : GameState
    {
        public PlayerTurn(GameLogic gameLogic, IInputHandler playerInputHandler) : base(gameLogic, playerInputHandler) { }

        public override void Activate()
        {
            _logic.CameraStatement.ToEnemyCam();
        }

        public override void Update()
        {
            _playerInputHandler.TrackInput();
        }


    }

    public class EnemyTurn : GameState
    {
        public EnemyTurn(GameLogic gameLogic) : base(gameLogic, null) { }


    }
}
