using System;
using UnityEngine;
using UnityEngine.UI;

namespace ParticleSystemInUi
{
    [RequireComponent(typeof(Button))]
    public class ButtonWithUIEffect : MonoBehaviour
    {
        private Button _button;
        private RectTransform _rectTransform;
    
        public event Action<RectTransform> OnClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            OnClicked?.Invoke(_rectTransform);
        }
    }
}