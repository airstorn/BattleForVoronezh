using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Data", menuName = "Battle/New Units Data")]
public class UnitsData : ScriptableObject
{
    public GridUnit[] Data;

    public GridUnit GetUnit<T>() where T : GridUnit
    {
        return Data.OfType<T>().First();
    }
}
