using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CheckPlayerInFOVRange : Node
{
    private Transform _transform;
    private List<Tile> path;
    private EnemyBT _enemyBT;

    public CheckPlayerInFOVRange(Transform transform)
    {
        _transform = transform;
        _enemyBT = EnemyBT.instance;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("checking fov range" + GetData("target"));
        object t = GetData("target");

        if (t == null)
        {
            // no current target, get all players in the game
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            if (playerObjects.Length > 0)
            {
                // for each found player, check if in range according to tile distance
                Tile currentTile = _enemyBT.tileSystem.GetTileFromPosition(_transform.position);

                foreach (GameObject playerObject in playerObjects)
                {
                    Vector3 playerPosition = playerObject.transform.position;
                    Tile playerTile = _enemyBT.tileSystem.GetTileFromPosition(playerPosition);

                    // Find path to player
                    path = PathFindingTestTwo.FindPath(currentTile, _enemyBT.tileSystem, _enemyBT.actionPoints, playerTile);

                    if (path != null)
                    {
                        path.RemoveAt(path.Count - 1);

                        if (path.Count <= _enemyBT.fovRangeTiles)
                        {
                            _enemyBT.path = path;
                            parent.parent.SetData("target", playerObject.transform);
                            state = NodeState.SUCCESS;
                            return state;
                        }
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






// private static int _playerLayerMask = 1 << 6;

// private Transform _transform;

// public CheckPlayerInFOVRange(Transform transform)
// {
//     _transform = transform;
// }

// public override NodeState Evaluate()
// {
//     object t = GetData("target");
//     if (t == null)
//     {
//         Collider[] colliders =
//             Physics
//                 .OverlapSphere(_transform.position,
//                 EnemyBT.fovRange,
//                 _playerLayerMask);

//         if (colliders.Length > 0)
//         {
//             Debug.Log("foundplayer");
//             parent.parent.SetData("target", colliders[0].transform);
//             state = NodeState.SUCCESS;
//             return state;
//         }

//         state = NodeState.FAILURE;
//         return state;
//     }

//     state = NodeState.SUCCESS;
//     return state;
// }

