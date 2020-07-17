using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityLevel
{
    level1 = 1,
    level2 = 2,
    level3 = 3
}

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private AbilityLevel _level = AbilityLevel.level1;
    [SerializeField] private int _count;

    public abstract void Init();
}
