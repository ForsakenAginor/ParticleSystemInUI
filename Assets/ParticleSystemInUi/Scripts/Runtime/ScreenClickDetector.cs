using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParticleSystemInUi
{
    [RequireComponent(typeof(Image))]
    public class ScreenClickDetector : MonoBehaviour, IPointerClickHandler
    {
        private RectTransform _rectTransform;

        public event Action<RectTransform, Vector2> ScreenClicked;
    
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ScreenClicked?.Invoke(_rectTransform, eventData.position);
        }
    }
}