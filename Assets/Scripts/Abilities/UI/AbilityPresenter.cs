using System.Collections.Generic;
using Abilities.Core;
using UnityEngine;

namespace Abilities.UI
{
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        private List<AbilityButton> _data = new List<AbilityButton>();
        
        public void SetVisible(bool visible)
        {
            _parent.gameObject.SetActive(visible);
        }

        public void CreateAbilityButton(GameObject buttonTemplate, VisualData data)
        {
            var obj = Instantiate(buttonTemplate);
            obj.GetComponent<AbilityButton>().SetData(data);
            obj.transform.SetParent(_parent);
            obj.GetComponent<RectTransform>().localScale = Vector3.one;

        }
    }
}
