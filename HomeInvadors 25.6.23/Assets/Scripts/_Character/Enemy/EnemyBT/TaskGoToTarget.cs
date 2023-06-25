using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToTarget : Node
{
    private Transform _transform;

    private EnemyBT _enemyBT;

    // private List<Tile> path;
    private int currentTileIndex = 0;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("going to target");
        if (
            currentTileIndex < _enemyBT.path.Count &&
            _enemyBT.GetRemainingActionPoints() > 0
        )
        {
            Tile currentTile = _enemyBT.path[currentTileIndex];
            Vector3 targetPosition =
                new Vector3(currentTile.gridX * _enemyBT.tileSystem.tileSize,
                    _transform.position.y,
                    currentTile.gridY * _enemyBT.tileSystem.tileSize);

            // Move towards the current tile
            _transform.position =
                Vector3
                    .MoveTowards(_transform.position,
                    currentTile.transform.position,
                    _enemyBT.moveSpeed * Time.deltaTime);
            Vector3 direction =
                (targetPosition - _transform.position).normalized;
            if (direction != Vector3.zero)
            {
                _transform.rotation = Quaternion.LookRotation(direction);
            }

            if (
                Vector3
                    .Distance(_transform.position,
                    currentTile.transform.position) <
                0.1f
            )
            {
                // Reached the current tile, move to the next tile
                _transform.position = targetPosition;
                currentTileIndex++;
                _enemyBT.tilesMoved++;
            }
        }
        else
        {
            if (_enemyBT.GetRemainingActionPoints() <= 0)
            {
                _enemyBT.path = null;
                currentTileIndex = 0;
                parent.parent.SetData("target", null);

                _enemyBT.EndTurn();
            }
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
