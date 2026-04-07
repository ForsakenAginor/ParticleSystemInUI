using System;
using UnityEngine;

namespace ParticleSystemInUi
{
    public class DemoLogic : MonoBehaviour
    {
        [SerializeField] private UIEffectPool _effectPool;
        [SerializeField] private RectTransform _defaultEffectCanvas;
        [SerializeField] private ButtonWithUIEffect[] _commonButtons;
        [SerializeField] private ButtonWithUIEffect[] _gridButtons;
        [SerializeField] private UIElementWithHighlightEffect[] _highlightButtons;
        [SerializeField] private ScreenClickDetector _screenClickDetector;

        private void Start()
        {
            foreach (ButtonWithUIEffect button in _commonButtons)
            {
                button.OnClicked += OnButtonClicked;
            }

            foreach (ButtonWithUIEffect button in _gridButtons)
            {
                button.OnClicked += OnGridButtonClicked;
            }

            foreach (UIElementWithHighlightEffect button in _highlightButtons)
            {
                button.PointerEnter += StartHightlight;
            }

            _screenClickDetector.ScreenClicked += OnScreenClicked;
        }

        private void OnDestroy()
        {
            foreach (ButtonWithUIEffect button in _commonButtons)
            {
                button.OnClicked -= OnButtonClicked;
            }

            foreach (ButtonWithUIEffect button in _gridButtons)
            {
                button.OnClicked -= OnGridButtonClicked;
            }

            foreach (UIElementWithHighlightEffect button in _highlightButtons)
            {
                button.PointerEnter += StartHightlight;
            }

            _screenClickDetector.ScreenClicked -= OnScreenClicked;
        }

        private void OnScreenClicked(RectTransform rect, Vector2 position)
        {
            _effectPool.GetUIEffectAdapter().PlayClickEffect(rect, position);
        }

        private void StartHightlight(RectTransform rect, Action<UIEffectAdapter> callback)
        {
            var adapter = _effectPool.GetUIEffectAdapter();
            callback.Invoke(adapter);
            adapter.PlayHighlightEffect(rect);
        }

        private void OnGridButtonClicked(RectTransform rectTransform)
        {
            _effectPool.GetUIEffectAdapter().PlayEffect(rectTransform, _defaultEffectCanvas);
        }

        private void OnButtonClicked(RectTransform rectTransform)
        {
            _effectPool.GetUIEffectAdapter().PlayEffect(rectTransform);
        }
    }
}