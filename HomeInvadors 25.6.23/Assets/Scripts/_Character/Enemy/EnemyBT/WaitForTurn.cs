using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class WaitForTurn : Node
{
    private Transform _transform;

    public WaitForTurn(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.FAILURE;
        return state;
    }
}
