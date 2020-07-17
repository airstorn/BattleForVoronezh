using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitiesDirector : MonoBehaviour
{
    [SerializeField] private List<Ability> _data = new List<Ability>();

    private void Awake()
    {
        _data = GetComponents<Ability>().ToList();
    }

    public Ability GetAbility<T>() where T : Ability
    {
        return _data.OfType<T>().First();
    }
}
