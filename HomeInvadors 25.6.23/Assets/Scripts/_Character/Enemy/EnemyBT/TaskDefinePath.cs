using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskDefinePath : Node
{
    private Transform _transform;

    private List<Tile> path;

    private EnemyBT _enemyBT;

    public TaskDefinePath(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("making path to trigger");
        object t = GetData("trigger");

        if (t == null)
        {
            // no current target, get all players in game
            GameObject[] playerObjects;
            playerObjects = GameObject.FindGameObjectsWithTag("Player");

            if (playerObjects.Length > 0)
            {
                // for each found player, check if in range according to tile distance
                Tile currentTile =
                    _enemyBT
                        .tileSystem
                        .GetTileFromPosition(_transform.position);
                foreach (GameObject playerObject in playerObjects)
                {
                    Tile endTile =
                        _enemyBT
                            .tileSystem
                            .GetTileFromPosition(playerObject
                                .transform
                                .position);
                    path =
                        PathFindingTestTwo
                            .FindPath(currentTile,
                            _enemyBT.tileSystem,
                            _enemyBT.GetRemainingActionPoints(),endTile);
                    path.RemoveAt(path.Count - 1);

                    if (path.Count <= _enemyBT.fovRangeTiles)
                    {
                        _enemyBT.path = path;
                        parent
                            .parent
                            .SetData("trigger", playerObject.transform);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
