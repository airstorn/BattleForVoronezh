using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSprites : MonoBehaviour
{
    [SerializeField] private Sprite _normal;
    [SerializeField] private Sprite _damaged;
    [SerializeField] private Sprite _missed;

    public Sprite Normal => _normal;
    public Sprite Damaged => _damaged;
    public Sprite Missed => _missed;

    public enum SpriteState
    {
        normal,
        damaged,
        missed
    }
    
    public static GridSprites Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSprite(SpriteState state)
    {
        switch (state)
        {
            case SpriteState.damaged:
                return _damaged;
            case SpriteState.missed:
                return _missed;
            case SpriteState.normal:
                return _normal;
            default:
                return _normal;
        }
    }
}
