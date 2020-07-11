using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LimitedShotsHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GridObject _interactionGrid;
    [SerializeField] private GameObject _enemyTurn;

    [SerializeField] private GameObject _template;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _additionalShots = 0;

    private Text _shotsText;
    private int _shotsCount;
    private GridElement _selectedElement;

    private ILevelTarget _playerTarget;
    private IShotable _shotBehaviour;
    private IGameState _nextState;
    private bool animate = false;
    private IEnumerator Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        _nextState = _enemyTurn.GetComponent<IGameState>();
        _playerTarget = GetComponent<ILevelTarget>();

        _shotsText = Instantiate(_template, _parent).GetComponent<Text>();

        yield return new WaitForEndOfFrame();
        
        _shotsCount = _interactionGrid.Units.Sum(unit => unit.Health.Total) * (_interactionGrid.Sheet.GetLength(0) + _interactionGrid.Sheet.GetLength(1)) / 2;
        _shotsCount += _additionalShots;
        Debug.Log(_interactionGrid.Units.Count);
        UpdateShotsCount();
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

    private void RemoveShot()
    {
        _shotsCount--;
        UpdateShotsCount();
    }

    private void UpdateShotsCount()
    {
        _shotsText.text = "Боеприпасы: " + _shotsCount;
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
        RemoveShot();
        yield return new WaitForSeconds(0.8f);

        if (_playerTarget.CheckTarget() == true)
        {
            _gameLogic.OnPlayerWin?.Invoke();
            yield break;
        }
        
        if (_shotsCount <= 0)
        {
            _gameLogic.OnPlayerLoose?.Invoke();
            yield break;
        }
        
        if(_selectedElement.HitState == GridSprites.SpriteState.missed)
            _gameLogic.ChangeState(_nextState);

        animate = false;
    }
}
