using System;
using UnityEngine;

public class WaitWhileCached : CustomYieldInstruction
{
    private Func<bool> _predicate;

    public WaitWhileCached(Func<bool> predicate)
    {
        _predicate = predicate;
    }

    public override bool keepWaiting => _predicate();

    public void UpdateCondition(Func<bool> newPredicate)
    {
        _predicate = newPredicate;
    }
}