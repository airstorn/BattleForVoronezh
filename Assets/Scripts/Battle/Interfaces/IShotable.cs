using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShotable
{
    void Release(Vector3 spawnPosition, ref GridElement success);
}
