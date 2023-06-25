using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public TileSelectHighlight tileSelectHighlight;
    public CameraManager cameraManager;

    public override void PlayTurn()
    {
        base.PlayTurn();
        HandleInput();

        if (tileSelected && path != null && path.Count > 0)
        {
            MoveCharacter();
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        tileSelected = false;
        tileSelectHighlight.UpdateReachableTiles(reachableTiles);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !tileSelected)
        {
            TileSelect();
        }
        if (!tileSelected)
        {
            CheckTileSelectionHover();
        }
    }

    private void CheckTileSelectionHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileSelectHighlight.raycastLayerMask))
        {
            Tile hitTile = hit.collider.GetComponent<Tile>();
            if (hitTile != null && reachableTiles.Contains(hitTile))
            {
                tileSelectHighlight.ClearPath(path);
                path = PathFindingTestTwo.RetracePath(currentTile, hitTile);
                tileSelectHighlight.HighlightTile(currentTile, hitTile);

            }
            else if (tileSelectHighlight.IsTileUnreachable(hitTile, actionPoints))
            {
                tileSelectHighlight.ClearPath(path);
            }
        }
        else
        {
            tileSelectHighlight.ClearPath(path);
        }
        tileSelectHighlight.UpdateReachableTiles(reachableTiles);
        tileSelectHighlight.HighlightPath(path, actionPoints);
    }

    private void TileSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = ~(1 << LayerMask.NameToLayer("walls") | 1 << LayerMask.NameToLayer("doors") | 1 << LayerMask.NameToLayer("event")); // Exclude the walls, doors and event layers

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Tile clickedTile = hit.collider.GetComponent<Tile>();

            if (clickedTile != null && reachableTiles.Contains(clickedTile))
            {
                tileSelectHighlight.SelectTile(clickedTile);
                currentPathIndex = 0;
                tilesMoved = 0;
                tileSelected = true;
            }
        }
    }
}