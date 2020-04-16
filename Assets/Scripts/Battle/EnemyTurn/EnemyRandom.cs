using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandom : MonoBehaviour
{
   [SerializeField] private IShotable _shot;
   [SerializeField] private GridObject _interactionGrid;

   private GridElement _selectedElement;
   private void Start()
   {
      _shot = GetComponent<IShotable>();
   }

   public IEnumerator ShootAtRandomPoint()
   {
     Shoot();
   
      while(_selectedElement.HitState == GridSprites.SpriteState.damaged)
      {
         yield return new WaitForSeconds(1f);
         Shoot();
      }
     
     yield return new WaitForSeconds(2);
   }

   private void Shoot()
   {
      Vector2Int randomPointId = RandomizedPoint();
      _selectedElement = _interactionGrid.Sheet[randomPointId.x, randomPointId.y];
      
      while (_selectedElement.HitState != GridSprites.SpriteState.normal)
      {
         randomPointId = RandomizedPoint();
         _selectedElement = _interactionGrid.Sheet[randomPointId.x, randomPointId.y];
      }
      
      _shot.Release(_interactionGrid.Sheet[randomPointId.x, randomPointId.y].CellPos, ref _selectedElement);
   }

   private Vector2Int RandomizedPoint()
   {
      return new Vector2Int(Random.Range(0, _interactionGrid.Sheet.GetLength(0)),Random.Range(0, _interactionGrid.Sheet.GetLength(1) ));
   }
}
