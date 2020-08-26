using Abilities.Core;
using GUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Shop
{
    public class ShopGradableItem : MonoBehaviour, IItemBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _level;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _image;
        private Button _button;
        private IDataReceiver<ShopData> _reference;
        private ShopData _containedData;

        public void SetItem(IAbilityData obj)
        {
            _containedData = (ShopData) obj;
            
            if (_reference == null)
                _reference = _containedData.Visual.Reference as IDataReceiver<ShopData>;
            
            _level.text = GetLevelString(_containedData.Visual.Level);
            _image.sprite = _containedData.Visual.Data.Icon;
            _name.text = Lean.Localization.LeanLocalization.GetTranslationText(_containedData.Visual.Data.Name);
            _priceText.text = "<sprite=0> " + _containedData.GradePrice;

            var gradable = _containedData.Visual.Reference as IGradable;
            _button = GetComponent<Button>();
            
            _button.onClick.RemoveAllListeners();

            _button.onClick.AddListener(gradable.Upgrade);
            _button.onClick.AddListener(RefreshData);
        }

        public void Refresh()
        {
            _name.text = Lean.Localization.LeanLocalization.GetTranslationText(_containedData.Visual.Data.Name);
        }

        private void RefreshData()
        {
            _reference.Interact(new ShopData(), CallbackConverter);
        }

        private void CallbackConverter(IAbilityData obj)
        {
            SetItem((ShopData)obj);
        }
        
        private string GetLevelString(AbilityLevel lvl)
        {
            switch (lvl)
            {
                case AbilityLevel.level1:
                    return "ур. 1";
                case AbilityLevel.level2:
                    return "ур. 2";
                case AbilityLevel.level3:
                    return "ур. 3";
                
                default:
                    return "Undifiened";
            }
        }
    }
}
