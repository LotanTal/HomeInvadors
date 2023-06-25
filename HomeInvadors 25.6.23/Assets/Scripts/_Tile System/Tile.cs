using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public bool isWalkable;
    [SerializeField] public int movementCost;
    [SerializeField] public int gridX;
    [SerializeField] public int gridY;

    public Tile parent { get; set; }
    public Transform occupiedBy;

    public int fCost => gCost + hCost;
    public int gCost { get; set; }
    public int hCost { get; set; }

    private const float DetectionFactor = 0.5f;

    public bool wallOnRight;
    public bool wallOnLeft;
    public bool wallOnTop;
    public bool wallOnBottom;
    public float tileSize;

    /// <summary>
    /// Called on the frame when a script is initialized.
    /// </summary>
    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            tileSize = collider.bounds.size.x;
        }
        else
        {
            Debug.LogError("No Collider attached to the GameObject.");
        }
    }

    /// <summary>
    /// Calculates the H cost for A* pathfinding.
    /// </summary>
    public void CalculateHCost(Tile endTile)
    {
        hCost = Mathf.Abs(gridX - endTile.gridX) + Mathf.Abs(gridY - endTile.gridY);
    }

    /// <summary>
    /// Checks for walls near the tile.
    /// </summary>
    public void CheckNearbyWalls(float tileSize)
    {
        int wallLayer = LayerMask.NameToLayer("walls");
        float detectionDistance = tileSize * DetectionFactor;

        // Get all colliders in a sphere around the tile's position
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionDistance);

        // Check each collider to see if it's on the "walls" layer and if it's close enough to the tile's edges
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == wallLayer)
            {
                Vector3 closestPoint = collider.ClosestPoint(transform.position);
                float distance = Vector3.Distance(closestPoint, transform.position);

                // Check if the collider is close enough to any part of the tile to be considered "nearby"
                if (distance < tileSize)
                {
                    DetermineWallEdge(closestPoint);
                }
            }
        }
    }

    /// <summary>
    /// Determines which edge of the tile a wall is closest to.
    /// </summary>
    private void DetermineWallEdge(Vector3 closestPoint)
    {
        bool isCloserToHorizontal = Mathf.Abs(closestPoint.x - transform.position.x) < Mathf.Abs(closestPoint.z - transform.position.z);

        if (isCloserToHorizontal)
        {
            wallOnBottom = closestPoint.z < transform.position.z;
            wallOnTop = !wallOnBottom;
        }
        else
        {
            wallOnLeft = closestPoint.x < transform.position.x;
            wallOnRight = !wallOnLeft;
        }
    }
}