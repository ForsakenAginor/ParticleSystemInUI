using System;
using UnityEngine;

namespace ParticleSystemInUi
{
    /// <summary>
    /// A cached coroutine waiter that waits while a condition is true.
    /// Optimized to avoid garbage collection by reusing the same wait condition.
    /// </summary>
    public class WaitWhileCached : CustomYieldInstruction
    {
        private Func<bool> _predicate;

        /// <summary>
        /// Initializes a new instance of the WaitWhileCached class with the specified condition.
        /// </summary>
        /// <param name="predicate">The condition to evaluate each frame. The coroutine waits while this returns true.</param>
        public WaitWhileCached(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// Gets a value indicating whether the coroutine should keep waiting.
        /// Returns true while the condition is true, false when the condition becomes false.
        /// </summary>
        public override bool keepWaiting => _predicate();

        /// <summary>
        /// Updates the condition function to evaluate while waiting.
        /// </summary>
        /// <param name="newPredicate">The new condition function.</param>
        public void UpdateCondition(Func<bool> newPredicate)
        {
            _predicate = newPredicate;
        }
    }
}