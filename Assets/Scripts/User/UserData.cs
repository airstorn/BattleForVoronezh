using System;
using Abilities.Core;
using Core;
using UnityEngine;

namespace User
{
      public class UserData : MonoBehaviour
      {
            [SerializeField] private string _username;
      
            public AbilitiesDirector AbilitiesDirector => _abilities;
            public IResourcable<int> Money => _money;
            
            private AbilitiesDirector _abilities;
            private IResourcable<int> _money;

            private readonly string _moneyPath = "money";
      
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

                        Setup();

                        DontDestroyOnLoad(gameObject);
                  }
            }

            private void Setup()
            {
                  _abilities = GetComponentInChildren<AbilitiesDirector>();
                  _money = GetComponent<IResourcable<int>>();

                  InitMoney();
            }

            private void InitMoney()
            {
                  var savedMoney = PlayerPrefs.GetInt(_moneyPath, 0);
                  _money.Add(savedMoney);
                  _money.OnValueChanged += SaveMoney;
            }

            private void SaveMoney(int data) => PlayerPrefs.SetInt(_moneyPath, data);

            private void OnDestroy()
            {
                  _money.OnValueChanged -= SaveMoney;
            }
      }
}
