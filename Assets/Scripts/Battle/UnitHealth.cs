using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridUnit))]
public class UnitHealth : MonoBehaviour
{
    [SerializeField] private GridUnit _base;
    [SerializeField] private int _health;
    
    private void Start()
    {
        CalcHealth();
    }

    private void CalcHealth()
    {
        _health = _base.Size.x * _base.Size.y;
    }

    public void ApplyDamage()
    {
        _health--;

        if(_health <= 0)
        {
            _base.Visual.SetBroken(true);
        }
    }
}
