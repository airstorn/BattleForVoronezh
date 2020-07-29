using Abilities.Core;
using Core;
using GUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace GUI.Shop
{
    public class ShopConsumableItem : MonoBehaviour, IItemBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _count;
        [SerializeField] private TMP_Text _priceText;
        private Button _button;
        private ShopData _containedData;
        private IDataReceiver<ShopData> _reference;
        private IResourcable<int> _resourcable;

        public void SetItem(IAbilityData obj)
        {
            _containedData = (ShopData) obj;
            
            if (_reference == null)
                _reference = _containedData.Visual.Reference as IDataReceiver<ShopData>;

            if (_resourcable == null)
                _resourcable = _containedData.Visual.Reference as IResourcable<int>;
            
            _count.text = _containedData.Visual.Count + "";
            _name.text = _containedData.Visual.Name;
            _priceText.text = "<sprite=0> " + _containedData.BuyPrice;

            _button = GetComponent<Button>();
            
            _button.onClick.RemoveAllListeners();

            Debug.Log(_containedData.Visual.Reference);
            _button.onClick.AddListener(BuyItem);
        }

        private void BuyItem()
        {
            if (UserData.Instance.Money.Get() < _containedData.BuyPrice)
                return;

            _resourcable.Add(1);
            UserData.Instance.Money.Remove(_containedData.BuyPrice);
        
            _reference.Interact(new ShopData(), Callback);
        }

        private void Callback(IAbilityData obj)
        {
            SetItem(obj);
        }
    }
}
