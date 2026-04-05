using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIElementWithHighlightEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    private UIEffectAdapter _effectAdapter;
    
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