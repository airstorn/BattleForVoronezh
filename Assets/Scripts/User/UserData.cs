using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Core;
using UnityEngine;

public class UserData : MonoBehaviour
{
      [SerializeField] private string _username;
      public AbilitiesDirector AbilitiesDirector => _abilities;
      private AbilitiesDirector _abilities;

      public static UserData Instance;

      private void Awake()
      {
            if (Instance != null)
            {
                  Destroy(gameObject);
            }
            else
            {
                  Instance = this;
                  _abilities = GetComponentInChildren<AbilitiesDirector>();

                  DontDestroyOnLoad(gameObject);
            }
      }
}
