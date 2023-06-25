using System.Collections.Generic;
using UnityEngine;

public static class PathFindingTestTwo
{
    private const int DiagonalCost = 14;
    private const int StraightCost = 10;

    public static List<Tile> FindPath(Tile startTile, TileSystem tileSystem, int maxMovementPoints, Tile endTile = null)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        List<Tile> reachableTiles = new List<Tile>();

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = FindTileWithLowestFCost(openSet);

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (endTile != null && currentTile == endTile)
            {
                return RetracePath(startTile, endTile);
            }

            foreach (Tile neighbor in tileSystem.GetNeighbors(currentTile, true))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor) + neighbor.movementCost;

                if (newMovementCostToNeighbor <= maxMovementPoints && (!openSet.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = (endTile != null) ? GetDistance(neighbor, endTile) : 0;
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }

                    if (endTile == null)
                    {
                        reachableTiles.Add(neighbor);
                    }
                }
            }
        }

        // If endTile is null, return the reachable tiles.
        if (endTile == null)
        {
            return reachableTiles;
        }

        // If endTile was specified but a path was not found, return null.
        return null;
    }

    private static Tile FindTileWithLowestFCost(List<Tile> openSet)
    {
        Tile lowestFCostTile = openSet[0];

        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fCost < lowestFCostTile.fCost ||
                (openSet[i].fCost == lowestFCostTile.fCost && openSet[i].hCost < lowestFCostTile.hCost))
            {
                lowestFCostTile = openSet[i];
            }
        }

        return lowestFCostTile;
    }

    public static List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        return path;
    }

    public static int GetDistance(Tile tileA, Tile tileB)
    {
        int distanceX = Mathf.Abs(tileA.gridX - tileB.gridX);
        int distanceY = Mathf.Abs(tileA.gridY - tileB.gridY);

        return distanceX > distanceY ? DiagonalCost * distanceY + StraightCost * (distanceX - distanceY)
                                      : DiagonalCost * distanceX + StraightCost * (distanceY - distanceX);
    }

    public static List<Tile> FindReachableTiles(Tile startTile, int maxTilesPerRound, TileSystem tileSystem)
    {
        List<Tile> reachableTiles = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        List<Tile> openSet = new List<Tile> { startTile };
        int reachableTileRadius = maxTilesPerRound + 1;

        startTile.gCost = 0;

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            foreach (Tile neighbor in tileSystem.GetNeighbors(currentTile, true))
            {
                if (neighbor == startTile || !neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor) + neighbor.movementCost;
                if (newMovementCostToNeighbor <= reachableTileRadius * StraightCost && (!openSet.Contains(neighbor) || newMovementCostToNeighbor <= reachableTileRadius * StraightCost))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }

                    if (!reachableTiles.Contains(neighbor)) // Check for duplicates
                    {
                        reachableTiles.Add(neighbor);
                    }
                }
            }
        }

        return reachableTiles;
    }
}