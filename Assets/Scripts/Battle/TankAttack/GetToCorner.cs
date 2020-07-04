using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace levelTarget
{
    public class GetToCorner : MonoBehaviour, ILevelTarget
    {
        [SerializeField] private GridObject _grid;
        
        public bool CheckTarget()
        {
            for (int y = 0; y < _grid.Sheet.GetLength(1); y++)
            {
                var obj = _grid.Sheet[_grid.Sheet.GetLength(0) - 1, y];
                if (obj.HoldedUnit != null && obj.HoldedUnit.Health.IsDead == false)
                    return true;
            }

            return false;
        }
    }
}
