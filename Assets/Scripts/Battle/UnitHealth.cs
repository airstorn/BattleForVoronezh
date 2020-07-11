using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridUnit))]
public class UnitHealth : MonoBehaviour
{
    private GridUnit _base;
    [SerializeField] private int _health;

    public bool IsDead => _health <= 0;
    public int Total => CalcHealth();
    
    private void Start()
    {
        _base = GetComponent<GridUnit>();
        _health = CalcHealth();
    }

    private int CalcHealth()
    {
        return _base.Size.x * _base.Size.y;
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _base.Visual.SetBroken(true);
        _base.Borders.ForEach(element => element.SetSpriteType(GridSprites.SpriteState.missed));
    }
}
