using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShotable
{
    IEnumerator Release(Vector3 spawnPosition, bool success);
}
