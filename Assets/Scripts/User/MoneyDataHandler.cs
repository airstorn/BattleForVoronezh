using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace User
{
    public class MoneyDataHandler : MonoBehaviour, IResourceListener<IResourcable<int>, int>
    {
        [SerializeField] private TMP_Text _text;
        
        private void Start()
        {
            UserData.Instance.Money.OnValueChanged += UpdateData;
            UpdateData(UserData.Instance.Money.Get()); 
        }

        public void UpdateData(int data)
        {
            _text.text = "<sprite=0> " + data;
        }
    }
}
