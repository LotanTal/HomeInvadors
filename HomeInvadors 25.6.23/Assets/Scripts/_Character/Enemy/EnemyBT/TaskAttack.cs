using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskAttack : Node
{
    private Transform _lastTarget;

    bool lost = false;

    public TaskAttack(Transform transform)
    {
    }

    public override NodeState Evaluate()
    {
        if (!lost)
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                _lastTarget = target;
            }

            Debug.Log("You lost");
            GameObject.Find("GameUI/HUD/GameOver").SetActive(true);
            lost = true;

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
