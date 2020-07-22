using System.Collections;
using System.Collections.Generic;
using Battle.Interfaces;
using UnityEngine;

public class PlayerTurnInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GridObject _interactionGrid;
    [SerializeField] private GameObject _enemyTurn;
    
    private GridElement _selectedElement;
    private Camera _raycastCamera;

    private ILevelTarget<GridObject> _playerTarget;
    private IShotable _shotBehaviour;
    private IGameState _nextState;
    private bool animate = false;
    
    private void Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        _nextState = _enemyTurn.GetComponent<IGameState>();
        _raycastCamera = Camera.main;
        _playerTarget = GetComponent<ILevelTarget<GridObject>>();
        
        _playerTarget.SetTarget(LevelData.Instance.EnemyGrid);
    }
    
    public void TrackInput()
    {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && animate == false)
        {
            if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
            {
                if (hit.collider)
                {
                    Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                    _selectedElement = HighlightPoint(roundedPos);
                    
                    if(_selectedElement.HitState == GridSprites.SpriteState.normal)
                        Shoot();
                }
            }
        }
    }

    private GridElement HighlightPoint(Vector3Int pos)
    {
        return _interactionGrid.GetVacantElement(pos);
    }

    public void Shoot()
    {
        StartCoroutine(ShotAnimation());
    }

    private IEnumerator ShotAnimation()
    {
        animate = true;
        
        _shotBehaviour.Release( ref _selectedElement);
        yield return new WaitForSeconds(0.8f);

        if (_playerTarget.CheckTarget() == true)
        {
            LevelData.Instance.OnPlayerWin?.Invoke();
            yield break;
        }
        
        if(_selectedElement.HitState == GridSprites.SpriteState.missed)
            LevelData.Instance.ChangeState(_nextState);

        animate = false;
    }
}
