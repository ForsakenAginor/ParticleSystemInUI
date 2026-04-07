using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleSystemInUi
{
    /// <summary>
    /// Manages a pool of UIEffectAdapter instances for efficient reuse of UI effects.
    /// Prevents frequent instantiation and destruction of effect objects.
    /// </summary>
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

        /// <summary>
        /// Retrieves a UIEffectAdapter from the pool.
        /// </summary>
        /// <returns>
        /// An available UIEffectAdapter instance ready to use.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when there are no available adapters in the pool.
        /// </exception>
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
}