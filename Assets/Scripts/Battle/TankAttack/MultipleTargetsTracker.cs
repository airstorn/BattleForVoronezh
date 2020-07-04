using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleTargetsTracker : MonoBehaviour, IInputHandler
{
    public struct ShotData
    {
        public Vector3Int ShotPoint;
        public GameObject ShotObject;
        public GridElement Element;
    }

    [SerializeField] private GameLogic _gameLogic;
    
    [SerializeField] private GridObject _targetGrid;
    [SerializeField] private GameObject _enemyTurn;
    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GameObject _targetTemplate;
    private int _shotsCount = 0;
    private Queue<ShotData> _shotsQueue = new Queue<ShotData>();

    private Camera _raycastCamera;
    private ILevelTarget _playerTarget;
    private IShotable _shotBehaviour;
    private IGameState _nextState;
    private bool animate = false;

    private void Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        _nextState = _enemyTurn.GetComponent<IGameState>();
        _playerTarget = GetComponent<ILevelTarget>();
        _raycastCamera = Camera.main;
    }


    public void TrackInput()
    {
        _shotsCount = _targetGrid.Units.Count;
        
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Input.GetMouseButtonDown(0) && animate == false)
        {
            if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
            {
                if (hit.collider)
                {
                    Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                    GridElement selectedElement = HighlightPoint(roundedPos);

                    if (selectedElement.HitState == GridSprites.SpriteState.normal)
                    {
                        CreateShot(roundedPos, selectedElement);
                        if (_shotsCount == _shotsQueue.Count)
                        {
                            // StartCoroutine(ShotAnimation());
                            _gameLogic.ChangeState(_nextState);
                        }
                    }
                }
            }
        }
    }

    private void CreateShot(Vector3Int point, GridElement element)
    {
        ShotData data = new ShotData()
        {
            ShotPoint = point,
            Element = element
        };

        if (_shotsQueue.Any(shotData => shotData.ShotPoint == data.ShotPoint) == false)
        {
            var template = Instantiate(_targetTemplate);
            template.transform.position = _targetGrid.GetVacantElement(data.ShotPoint).CellPos;
            data.ShotObject = template;
            _shotsQueue.Enqueue(data);
        }
    }
    
    private GridElement HighlightPoint(Vector3Int pos)
    {
        return _targetGrid.GetVacantElement(pos);
    }
    
    public IEnumerator ShotAnimation()
    {
        animate = true;

        while (_shotsQueue.Count != 0)
        {
            var shot = _shotsQueue.Dequeue();
            Destroy(shot.ShotObject);
            _shotBehaviour.Release(ref shot.Element);
            
            yield return new WaitForSeconds(0.8f);

            if (_playerTarget.CheckTarget() == true)
            {
                _gameLogic.OnPlayerWin?.Invoke();
                yield break;
            }
        }
        
            //
            // if(shot.Element.HitState == GridSprites.SpriteState.missed)
           
      

        animate = false;
    }
}
