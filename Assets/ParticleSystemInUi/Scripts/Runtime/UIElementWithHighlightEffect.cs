using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Provides highlight effect functionality for UI elements on pointer enter/exit events.
/// Requires a RectTransform component and works with UIEffectAdapter for visual feedback.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIElementWithHighlightEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    private UIEffectAdapter _effectAdapter;
    
    /// <summary>
    /// Event triggered when the pointer enters the UI element.
    /// Provides the RectTransform of the element and a callback to set the effect adapter.
    /// </summary>
    public event Action<RectTransform, Action<UIEffectAdapter>> PointerEnter;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(_rectTransform, SetAdapter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _effectAdapter?.StopAllEffects();
    }

    private void SetAdapter(UIEffectAdapter adapter)
    {
        _effectAdapter = adapter;
    }
}