using System;
using Abilities.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Abilities.UI
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _countText;
        public IDataReceiver<VisualData> Reference => _reference;

        private Button _button;
        private IDataReceiver<VisualData> _reference; 

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetData(VisualData data)
        {
            Debug.Log(data.Data.Name);
            if(_nameText)
                _nameText.text = Lean.Localization.LeanLocalization.GetTranslationText(data.Data.Name);
            if(_countText)
                _countText.text = "" + data.Count;

            _reference = data.Reference as IDataReceiver<VisualData>;
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(data.Reference.Interact);
            _button.onClick.AddListener(UpdateData);
        }

        private void UpdateData()
        {
            _reference.Interact(new VisualData(), Callback);
        }

        private void Callback(IAbilityData obj)
        {
            SetData((VisualData)obj);
        }
    }
}
