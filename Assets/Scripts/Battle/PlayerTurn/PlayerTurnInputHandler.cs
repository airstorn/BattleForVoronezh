using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactionGrid;
    [SerializeField] private Transform _pointVisualiser;
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private GameObject _enemyTurn;
    
    private GridElement _selectedElement;
    private IShotable _shotBehaviour;

    private void Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
    }
    
    public void TrackInput()
    {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
            {
                if (hit.collider != null)
                {
                    Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                    _pointVisualiser.position = roundedPos;
                    HighlightPoint(roundedPos);
                    Shoot();
                }
            }
        }
    }

    private void HighlightPoint(Vector3Int pos)
    {
        _interactionGrid.UpdateGridEngagements();
        var unit = _interactionGrid.GetVacantElement(pos);
        unit.SetElementEngagement(GridObject.ElementState.vacant);

        _selectedElement = unit;
    }

    public void Shoot()
    {
        StartCoroutine(ShotAnimation());
    }

    private IEnumerator ShotAnimation()
    {
        if (_selectedElement.HoldedUnit != null)
        {
            var enemyHealth = _selectedElement.HoldedUnit.GetComponent<UnitHealth>();
            enemyHealth.ApplyDamage();
        }
        yield return _shotBehaviour.Release(_selectedElement.CellPos, _selectedElement.HoldedUnit != null);
        _gameLogic.ChangeState(_enemyTurn.GetComponent<IGameState>());
    }
}
