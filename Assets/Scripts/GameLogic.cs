using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private UnitsManager _unitsManagement;
    [SerializeField] private GridObject _PlayerGrid;
    [SerializeField] private GridObject _EnemyGrid;
    [SerializeField] private CameraTurns _cameraStatement;
 
    public UnitsManager UnitsManager => _unitsManagement;
    public CameraTurns CameraStatement => _cameraStatement;

    public GameStates.PlayerTurn _state_PlayerTurn;
    public GameStates.EnemyTurn _state_EnemyTurn;
    public GameStates.PlaceUnits _state_PlaceUnits;

    private GameState _state;


    private void Start()
    {
        _state_EnemyTurn = new GameStates.EnemyTurn(this);
        _state_PlaceUnits = new GameStates.PlaceUnits(this);
        _state_PlayerTurn = new GameStates.PlayerTurn(this);

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
    }

    public void Confirm_button()
    {
        ChangeState(_state_PlayerTurn);
    }
}

public class GameState
{
    protected GameLogic _logic;

    public GameState(GameLogic gameLogic)
    {
        _logic = gameLogic;
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void Update() { }
}

namespace GameStates
{
    public class PlaceUnits : GameState
    {
        public PlaceUnits(GameLogic gameLogic) : base(gameLogic) { }

        public override void Activate()
        {
            _logic.StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _logic.CameraStatement.ToPlacerCam();
            _logic.UnitsManager.InitUnits();
            yield return null;
            //_logic.ChangeState(_logic._state_PlayerTurn);
        }

        public override void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _logic.UnitsManager.CatchUnit();
            }
        }
    }

    public class PlayerTurn : GameState
    {
        public PlayerTurn(GameLogic gameLogic) : base(gameLogic) { }

        public override void Activate()
        {
            _logic.CameraStatement.ToEnemyCam();
        }
    }

    public class EnemyTurn : GameState
    {
        public EnemyTurn(GameLogic gameLogic) : base(gameLogic) { }


    }
}
