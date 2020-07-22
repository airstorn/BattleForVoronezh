using System;
using Abilities.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities.UI
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private Text _nameText;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetData(VisualData data)
        {
            _nameText.text = data.Name;
            _button.onClick.AddListener(data.Reference.Interact);
        }
    }
}
