using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CheckTriggerActive : Node
{
    private Transform _transform;
    private EnemyBT _enemyBT;

    public CheckTriggerActive(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        object triggerData = GetData("trigger");

        if (triggerData == null)
        {
            Trigger[] triggers = Resources.FindObjectsOfTypeAll<Trigger>();

            foreach (Trigger trigger in triggers)
            {
                if (CanReachTrigger(trigger))
                {
                    if (TryFindPathToTrigger(trigger))
                    {
                        Debug.Log($"Path found with {_enemyBT.path.Count} tiles");
                    }
                    else
                    {
                        Debug.Log("No valid path found or not enough movement points.");
                        FindClosestTileToTrigger(trigger);
                    }

                    parent.parent.SetData("trigger", trigger.transform);
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }

    private bool CanReachTrigger(Trigger trigger)
    {
        float distance = Vector3.Distance(_transform.position, trigger.transform.position);
        return distance <= trigger.radiusOfEffect && trigger.isActive;
    }

    private bool TryFindPathToTrigger(Trigger trigger)
    {
        Tile currentTile = _enemyBT.tileSystem.GetTileFromPosition(_transform.position);
        Vector3 triggerTilePos = new Vector3(trigger.transform.position.x, trigger.transform.position.y, trigger.transform.position.z + 1f);
        Tile endTile = _enemyBT.tileSystem.GetTileFromPosition(triggerTilePos);

        _enemyBT.path = _enemyBT.FindPathToTarget(currentTile, endTile);
        return _enemyBT.path != null;
    }

    private void FindClosestTileToTrigger(Trigger trigger)
    {
        Tile currentTile = _enemyBT.tileSystem.GetTileFromPosition(_transform.position);
        Tile closestTile = null;
        float closestDistance = float.MaxValue;

        foreach (Tile tile in _enemyBT.reachableTiles)
        {
            float distance = Vector3.Distance(tile.transform.position, trigger.transform.position);
            if (distance < closestDistance)
            {
                closestTile = tile;
                closestDistance = distance;
            }
        }

        if (closestTile != null)
        {
            _enemyBT.path = PathFindingTestTwo.RetracePath(currentTile, closestTile);
            _enemyBT.tileSelected = true;
        }
    }
}
