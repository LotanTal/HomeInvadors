using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectHighlight : MonoBehaviour
{
    [SerializeField] public Material reachableTileMaterial;
    [SerializeField] public Material pathMaterial;
    [SerializeField] public Material defaultMaterial;
    [SerializeField] public Material selectedMaterial;
    [SerializeField] public Player character;
    [SerializeField] public TileSystem tileSystem;
    [SerializeField] public LayerMask raycastLayerMask;

    public Tile currentTile;
    public Tile selectedTile;
    public Tile highlightedTile;

    public List<Tile> tilepath = new List<Tile>();
    public List<Tile> reachableTiles = new List<Tile>();
    public List<Tile> highlightedTiles = new List<Tile>();

    public void UpdateReachableTiles(List<Tile> reachableTiles)
    {
        ClearHighlightedTiles();
        highlightedTiles.AddRange(reachableTiles);
        HighlightReachableTiles(reachableTiles);
        Debug.Log("reachableTiles: " + reachableTiles.Count);
    }

    public void HighlightReachableTiles(List<Tile> reachableTiles)
    {
        foreach (Tile tile in reachableTiles)
        {
            if (tile != selectedTile && tile != highlightedTile && highlightedTiles.Contains(tile))
            {
                SetTileMaterial(tile, reachableTileMaterial);
            }
        }
    }

    public void HighlightPath(List<Tile> tilepath, int maxTilesPerRound)
    {
        if (tilepath == null) return;

        for (int i = 0; i < tilepath.Count; i++)
        {
            Tile tile = tilepath[i];
            if (tile != selectedTile)
            {
                SetTileMaterial(tile, pathMaterial);
                if (i < maxTilesPerRound) highlightedTiles.Add(tile);
            }
        }
    }

    public void SetTileMaterial(Tile tile, Material material)
    {
        if (tile == null)
        {
            // Debug.LogWarning("SetTileMaterial: tile is null.");
            return;
        }

        Renderer renderer = tile.GetComponent<Renderer>();

        if (renderer == null)
        {
            // Debug.LogWarning("SetTileMaterial: Renderer component is missing from the tile.");
            return;
        }

        if (material == null)
        {
            // Debug.LogWarning("SetTileMaterial: material is null.");
            return;
        }

        renderer.material = material;
    }

    public void ClearHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
        {
            SetTileMaterial(tile, defaultMaterial);
        }
        highlightedTiles.Clear();
        reachableTiles.Clear();
    }

    public void ClearPath(List<Tile> tilepath)
    {
        if (highlightedTile != null)
        {
            SetTileMaterial(highlightedTile, pathMaterial);
        }

        highlightedTile = null;

        foreach (Tile tile in tilepath)
        {
            SetTileMaterial(tile, defaultMaterial);
        }

        tilepath.Clear();
    }

    public bool IsTileUnreachable(Tile tile, int maxTilesPerRound)
    {
        return tilepath == null || tilepath.Count >= maxTilesPerRound || character.actionPoints < maxTilesPerRound;
    }

    public void HighlightTile(Tile currentTile, Tile tile)
    {
        SetTileMaterial(highlightedTile, reachableTileMaterial);
        int distance = PathFindingTestTwo.GetDistance(currentTile, tile);
        Material material = (distance <= character.GetRemainingActionPoints() && tile.isWalkable) ? reachableTileMaterial : defaultMaterial;
        SetTileMaterial(tile, material);
        highlightedTile = tile;
    }

    public void SelectTile(Tile tile)
    {
        SetTileMaterial(selectedTile, defaultMaterial);
        selectedTile = tile;
        highlightedTile = null;
    }
}
