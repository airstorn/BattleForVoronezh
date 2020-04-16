using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactionGrid;
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private GameObject _enemyTurn;
    
    private GridElement _selectedElement;
    private IShotable _shotBehaviour;
    private IGameState _nextState;
    private bool animate = false;
    private void Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        _nextState = _enemyTurn.GetComponent<IGameState>();
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
        
        _shotBehaviour.Release(_selectedElement.CellPos, ref _selectedElement);
        yield return new WaitForSeconds(2);
        
        if(_selectedElement.HitState == GridSprites.SpriteState.missed)
            _gameLogic.ChangeState(_nextState);

        animate = false;
    }
}
