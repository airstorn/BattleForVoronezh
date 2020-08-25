using GameStates;
using GUI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

    public class ClickSoundInvoker : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SoundType _type = SoundType.Button;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            SoundsPlayer.Instance.PlaySound(_type);
        }
}