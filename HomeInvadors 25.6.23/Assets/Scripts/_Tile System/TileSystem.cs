using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField] public int gridSize;
    [SerializeField] public float tileSize;
    [SerializeField] public Transform tilePrefab;

    public List<Tile> tiles { get; private set; }

    private const int ObstacleLayerIndex = 7;
    private const float RaycastOffset = 100f;

    private void Awake()
    {
        CreateGrid();
    }

    // Creates a grid of tiles.
    private void CreateGrid()
    {
        tiles = new List<Tile>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                CreateTile(x, y);
            }
        }
    }

    // Creates a tile at the specified grid position.
    private void CreateTile(int x, int y)
    {
        Vector3 tilePosition = new Vector3(x * tileSize, 0, y * tileSize);
        RaycastHit hitInfo;

        bool isBlocked = Physics.Raycast(
            tilePosition + Vector3.up * RaycastOffset,
            Vector3.down,
            out hitInfo,
            Mathf.Infinity,
            1 << ObstacleLayerIndex
        );

        if (isBlocked)
        {
            CreateNonWalkableTile(x, y, hitInfo.collider.transform);
        }
        else
        {
            CreateWalkableTile(tilePosition, x, y);
        }
    }

    private void CreateNonWalkableTile(int x, int y, Transform obstacleTransform)
    {
        var tileObject = new GameObject($"Tile {x}, {y}");
        var tile = tileObject.AddComponent<Tile>();
        tileObject.AddComponent<BoxCollider>();

        tile.gridX = x;
        tile.gridY = y;
        tile.isWalkable = false;
        tile.movementCost = int.MaxValue;
        tile.occupiedBy = obstacleTransform;

        tiles.Add(tile);
    }

    private void CreateWalkableTile(Vector3 position, int x, int y)
    {
        var newTileTransform = Instantiate(tilePrefab, position, Quaternion.identity);
        newTileTransform.parent = transform;

        if (newTileTransform.TryGetComponent(out Tile tile))
        {
            tile.gridX = x;
            tile.gridY = y;
            tile.isWalkable = true;
            tile.movementCost = Random.Range(1, 4);

            tile.CheckNearbyWalls(tileSize);

            tiles.Add(tile);
        }
    }

    // Gets the tile at the specified world position.
    public Tile GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize);
        int y = Mathf.RoundToInt(position.z / tileSize);
        return tiles.Find(tile => tile.gridX == x && tile.gridY == y);
    }

    // Retrieves the path from the start tile to the end tile.
    private List<Tile> RetracePath(Tile startTile, Tile endTile)
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

    public List<Tile> GetNeighbors(Tile tile, bool considerWalls)
    {
        List<Tile> neighbors = new List<Tile>();

        // Check the four adjacent tiles
        TryAddNeighbor(tile, -1, 0, neighbors, considerWalls); // Left
        TryAddNeighbor(tile, 1, 0, neighbors, considerWalls);  // Right
        TryAddNeighbor(tile, 0, -1, neighbors, considerWalls); // Bottom
        TryAddNeighbor(tile, 0, 1, neighbors, considerWalls);  // Top

        return neighbors;
    }

    private void TryAddNeighbor(Tile tile, int offsetX, int offsetY, List<Tile> neighbors, bool considerWalls)
    {
        int neighborX = tile.gridX + offsetX;
        int neighborY = tile.gridY + offsetY;

        // Check bounds
        if (neighborX < 0 || neighborX >= gridSize || neighborY < 0 || neighborY >= gridSize)
        {
            return;
        }

        Tile neighborTile = tiles.Find(t => t.gridX == neighborX && t.gridY == neighborY);

        // Check if the tile is walkable and if there is no wall between tiles
        if (neighborTile.isWalkable && (!considerWalls || !IsWallBetweenTiles(tile, neighborTile)))
        {
            neighbors.Add(neighborTile);
        }
    }

    private bool IsWallBetweenTiles(Tile tileA, Tile tileB)
    {
        if ((tileA.wallOnLeft && tileB.gridX < tileA.gridX) || (tileA.wallOnRight && tileB.gridX > tileA.gridX))
        {
            return true;
        }

        if ((tileA.wallOnBottom && tileB.gridY < tileA.gridY) || (tileA.wallOnTop && tileB.gridY > tileA.gridY))
        {
            return true;
        }

        return false;
    }
}




