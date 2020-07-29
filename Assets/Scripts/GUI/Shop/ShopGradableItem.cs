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
        private Button _button;
        private IDataReceiver<ShopData> _reference;

        public void SetItem(IAbilityData obj)
        {
            ShopData data = (ShopData) obj;
            
            if (_reference == null)
                _reference = data.Visual.Reference as IDataReceiver<ShopData>;
            
            _level.text = GetLevelString(data.Visual.Level);
            _name.text = data.Visual.Name;
            _priceText.text = "<sprite=0> " + data.GradePrice;

            var gradable = data.Visual.Reference as IGradable;
            _button = GetComponent<Button>();
            
            _button.onClick.RemoveAllListeners();

            _button.onClick.AddListener(gradable.Upgrade);
            _button.onClick.AddListener(RefreshData);
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
