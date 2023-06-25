using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CheckPlayerInAttackRange : Node
{
    private Transform _transform;

    private List<Tile> path;

    private EnemyBT _enemyBT;

    public CheckPlayerInAttackRange(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("checking attack range");
        object t = GetData("target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        //get distance from player
        Transform target = (Transform) t;
        Tile currentTile =
            _enemyBT.tileSystem.GetTileFromPosition(_transform.position);
        Tile endTile = _enemyBT.tileSystem.GetTileFromPosition(target.position);

        path =
            PathFindingTestTwo
                // .FindPath(currentTile, endTile, _enemyBT.tileSystem);
                .FindPath(currentTile, _enemyBT.tileSystem, _enemyBT.actionPoints);

        if (path.Count <= _enemyBT.attackRangeTiles)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
