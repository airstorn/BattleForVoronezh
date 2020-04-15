using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandom : MonoBehaviour
{
   [SerializeField] private IShotable _shot;
   [SerializeField] private GridObject _interactionGrid;

   private void Start()
   {
      _shot = GetComponent<IShotable>();
   }

   public IEnumerator ShootAtRandomPoint()
   {
      Vector2Int randomPointId = new Vector2Int(Random.Range(0, _interactionGrid.Sheet.GetLength(0)),Random.Range(0, _interactionGrid.Sheet.GetLength(1) ));

     yield return _shot.Release(_interactionGrid.Sheet[randomPointId.x, randomPointId.y].CellPos, _interactionGrid.Sheet[randomPointId.x,randomPointId.y].HoldedUnit != null);
   }
}
