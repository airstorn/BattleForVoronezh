using Abilities.Core;
using UnityEngine;

namespace Abilities.UI
{
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _template;
        
        public void CreateAbilityButton(VisualData data)
        {
            Debug.Log("created");
            var obj = Instantiate(_template, _parent);
            obj.GetComponent<AbilityButton>().SetData(data);
        }
    }
}
