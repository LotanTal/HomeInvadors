using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskGoToTrigger : Node
{
    private Transform _transform;
    private EnemyBT _enemyBT;
    private Transform _triggerTransform;

    public TaskGoToTrigger(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        if (_enemyBT.path.Count > 0)
        {
            Debug.Log("Going to trigger " + _enemyBT.path[_enemyBT.path.Count - 1].gridX + " " + _enemyBT.path[_enemyBT.path.Count - 1].gridY);

            _enemyBT.MoveCharacter();

            if (_enemyBT.currentPathIndex >= _enemyBT.path.Count)
            {
                _enemyBT.path.Clear();
                parent.parent.SetData("trigger", null);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            _enemyBT.EndTurn();
            parent.parent.SetData("trigger", null);
            state = NodeState.SUCCESS;
            return state;
        }
    }
}