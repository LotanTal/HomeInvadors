using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskPatrol : Node
{
    private Transform _transform;

    private EnemyBT _enemyBT;

    private Transform[] _waypoints;

    private List<Tile> path;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        _enemyBT = EnemyBT.instance;
    }

    public void FindPathToTarget(Vector3 targetPosition)
    {
        Tile startTile =
            _enemyBT.tileSystem.GetTileFromPosition(_transform.position);
        Tile endTile = _enemyBT.tileSystem.GetTileFromPosition(targetPosition);

        if (startTile != endTile)
        {
            path =
                PathFindingTestTwo
                    .FindPath(startTile, _enemyBT.tileSystem, _enemyBT.actionPoints);
            _enemyBT.currentPathIndex = 0;
        }
    }

    public override NodeState Evaluate()
    {
        Debug.Log("patroling");

        // No waypoints to patrol
        if (_waypoints.Length == 0)
        {
            _enemyBT.EndTurn();
            state = NodeState.FAILURE;
            return state;
        }

        if (_enemyBT.GetRemainingActionPoints() <= 0)
        {
            _enemyBT.currentPathIndex = 0;
            _enemyBT.path = null;
            _enemyBT.EndTurn();
            state = NodeState.SUCCESS;
            return state;
        }

        if (_enemyBT.currentPathIndex < path.Count)
        {
            Tile currentTile = path[_enemyBT.currentPathIndex];
            Vector3 targetPosition =
                new Vector3(currentTile.gridX * _enemyBT.tileSystem.tileSize,
                    _transform.position.y,
                    currentTile.gridY * _enemyBT.tileSystem.tileSize);

            // Move towards the current tile
            _transform.position =
                Vector3
                    .MoveTowards(_transform.position,
                    targetPosition,
                    _enemyBT.moveSpeed * Time.deltaTime);
            Vector3 direction =
                (targetPosition - _transform.position).normalized;
            if (direction != Vector3.zero)
            {
                _transform.rotation = Quaternion.LookRotation(direction);
            }

            if (Vector3.Distance(_transform.position, targetPosition) < 0.1f)
            {
                // Reached the current tile, move to the next tile
                _transform.position = targetPosition;

                EnemyBT.currentWaypointIndex =
                    (EnemyBT.currentWaypointIndex + 1) % _waypoints.Length;
                _enemyBT.currentPathIndex++;
                _enemyBT.tilesMoved++;
            }
        }
        else
        {
            EnemyBT.currentWaypointIndex =
                (EnemyBT.currentWaypointIndex + 1) % _waypoints.Length;
            _enemyBT.currentPathIndex = 0;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
