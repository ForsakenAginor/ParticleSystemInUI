using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectPool : MonoBehaviour
{
    [SerializeField] private UIEffectAdapter[] _adapters;

    private Queue<UIEffectAdapter> _pool = new Queue<UIEffectAdapter>();

    private void Start()
    {
        for (int i = 0; i < _adapters.Length; i++)
        {
            _pool.Enqueue(_adapters[i]);
        }
    }

    public UIEffectAdapter GetUIEffectAdapter()
    {
        if (_pool.Count == 0)
            throw new Exception("No available UIEffect pool");

        var result = _pool.Dequeue();
        result.StopAllEffects();
        _pool.Enqueue(result);
        return result;
    }
}