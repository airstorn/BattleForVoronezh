using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
      [SerializeField] private string _username;
      [SerializeField] private int _coins;
      [SerializeField] private List<GridUnit> _units;

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
                  DontDestroyOnLoad(gameObject);
            }
      }
}
