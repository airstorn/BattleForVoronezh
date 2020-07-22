using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class LimitedShotsHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GameObject _enemyTurn;

    [SerializeField] private GameObject _template;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _additionalShots = 0;

    private Text _shotsText;
    private int _shotsCount;
    private GridElement _selectedElement;
    private Camera _raycastCamera;

    private ILevelTarget<GridObject> _target;
    private IShotable _shotBehaviour;
    private IGameState _nextState;
    private bool animate = false;
    private IEnumerator Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        _nextState = _enemyTurn.GetComponent<IGameState>();
        _target = GetComponent<ILevelTarget<GridObject>>();
        _raycastCamera = Camera.main;

        _shotsText = Instantiate(_template, _parent).GetComponent<Text>();

        yield return new WaitForEndOfFrame();

        var target = _target.GetTarget();
        
        _shotsCount = target.Units.Sum(unit => unit.Health.Total) * (target.Sheet.GetLength(0) + target.Sheet.GetLength(1)) / 2;
        _shotsCount += _additionalShots;
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
        return _target.GetTarget().GetVacantElement(pos);
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

        if (_target.CheckTarget() == true)
        {
            LevelData.Instance.OnPlayerWin?.Invoke();
            yield break;
        }
        
        if (_shotsCount <= 0)
        {
            LevelData.Instance.OnPlayerLoose?.Invoke();
            yield break;
        }
        
        if(_selectedElement.HitState == GridSprites.SpriteState.missed)
            LevelData.Instance.ChangeState(_nextState);

        animate = false;
    }
}
