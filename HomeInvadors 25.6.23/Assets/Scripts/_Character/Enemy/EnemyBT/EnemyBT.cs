using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : BehaviorTree.Tree
{
    public static EnemyBT instance;

    protected override Node SetupTree()
    {
        instance = this;
        tileSystem = FindObjectOfType<TileSystem>();
        Node root =
            new Selector(new List<Node> {
                    // new WaitForTurn(transform),
                    new Sequence(new List<Node> {
                            new CheckPlayerInAttackRange(transform),
                            new TaskAttack(transform)
                        }),
                    new Sequence(new List<Node> {
                            new CheckPlayerInFOVRange(transform),
                            new TaskGoToTarget(transform)
                        }),
                    new Sequence(new List<Node> {
                            new CheckTriggerActive(transform),
                            new TaskGoToTrigger(transform)
                        }),
                    new TaskPatrol(transform, waypoints)
                });

        return root;
    }

    public override void PlayTurn()
    {
        base.PlayTurn();
        Evaluate();
    }
}