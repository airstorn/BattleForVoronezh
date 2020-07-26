using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using Core;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultipleTargetsTracker : MonoBehaviour, IInitiatable<IPlayerState>, IInputHandler
{
    public struct ShotData
    {
        public Vector3Int ShotPoint;
        public GameObject ShotObject;
        public GridElement Element;
    }
    
    public event OnInputStopped OnInputStoppedHandler;

    [SerializeField] private LayerMask _raycastIgnore;
    [SerializeField] private GameObject _targetTemplate;
    
    private int _shotsCount = 0;
    private Queue<ShotData> _shotsQueue = new Queue<ShotData>();

    private Camera _raycastCamera;
    private EventSystem _eventSystem;
    private GridObject _targetGrid;
    private IShotable _shotBehaviour;
    private IPlayerState _playerState;
    private bool animate = false;

    private void Start()
    {
        _shotBehaviour = GetComponent<IShotable>();
        
        _targetGrid = LevelData.Instance.EnemyGrid;
        _raycastCamera = Camera.main;
        _eventSystem = EventSystem.current;
    }


    public void TrackInput()
    {
        // _shotsCount = _targetGrid.Units.Count(unit => unit.Health.IsDead == false);
        
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Input.GetMouseButtonDown(0) && animate == false && _eventSystem.IsPointerOverGameObject() == false)
        {
            if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
            {
                if (hit.collider)
                {
                    Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                    GridElement selectedElement = HighlightPoint(roundedPos);

                    if (selectedElement.HitState == GridSprites.SpriteState.normal || selectedElement.HitState == GridSprites.SpriteState.missed)
                    {
                        CreateShot(roundedPos, selectedElement);
                        if (_shotsCount == _shotsQueue.Count)
                        {
                            // StartCoroutine(ShotAnimation());
                            // LevelData.Instance.ChangeState(_nextState);
                            OnInputStoppedHandler?.Invoke();
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
    
    public IEnumerator ShotAnimation(Action callback)
    {
        animate = true;
        yield return new WaitForSeconds(0.2f);

        while (_shotsQueue.Count != 0)
        {
            var shot = _shotsQueue.Dequeue();
            Destroy(shot.ShotObject);
            _shotBehaviour.Release(ref shot.Element);
            
            yield return new WaitForSeconds(0.8f);

            // if (_playerTarget.CheckTarget() == true)
            // {
            //     LevelData.Instance.OnPlayerWin?.Invoke();
            //     yield break;
            // }
        }
        animate = false;
        
        callback?.Invoke();
    }

    public void Init(IPlayerState initiationObject)
    {
        _playerState = initiationObject;
    }

    public void SetShotsCount(int count)
    {
        _shotsCount = count;
    }
}
