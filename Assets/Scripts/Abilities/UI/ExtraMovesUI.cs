using System;
using Abilities.Behaviours;
using Abilities.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace Abilities.UI
{
    public class ExtraMovesUi : MonoBehaviour
    {
        private UserData _data;
        [SerializeField] private TMP_Text _priceText;

        public void Init(int price)
        {
            _data = UserData.Instance;
            _priceText.text = "1 ход - " + price + "<sprite=0>";
        }


        public void BuyMoveButton()
        {
            var v = _data.AbilitiesDirector.GetAbility(typeof(ExtraMoves));

            var inter = v as IDataReceiver<ExtraMovesData>;
            inter?.Interact(new ExtraMovesData(), Callback);
        }

        private void Callback(IAbilityData obj)
        {
        }
    }
}
