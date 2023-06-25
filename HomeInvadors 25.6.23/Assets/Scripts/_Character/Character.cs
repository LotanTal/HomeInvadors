using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Attributes")]
    public string characterName;
    public CharacterType characterType;
    public float moveSpeed;
    public int actionPoints;
    public int initialActionPoints;

    [Header("Tile Info")]
    public bool tileSelected = false;
    public List<Tile> path;
    public List<Tile> reachableTiles;
    public int tilesMoved;
    public int currentPathIndex = 0;
    public Tile currentTile;

    [Header("Camera")]
    public CinemachineVirtualCamera characterCam;

    [Header("Inventory")]
    public Inventory inventory;

    [Header("State")]
    public bool isCurrentlyPlaying;

    public TileSystem tileSystem;

    private void Awake()
    {
        tileSystem = FindObjectOfType<TileSystem>();
    }

    public virtual void FindReachableTiles(Tile currentTile, int maxTilesPerRound)
    {
        reachableTiles = PathFindingTestTwo.FindReachableTiles(currentTile, actionPoints, tileSystem);
    }

    public virtual List<Tile> FindPathToTarget(Tile startTile, Tile endTile)
    {
        if (endTile.isWalkable)
        {
            path = PathFindingTestTwo.FindPath(startTile, tileSystem, actionPoints, endTile);
        }
        return path;
    }

    public virtual void MoveCharacter()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            Tile targetTile = path[currentPathIndex];
            Vector3 targetPosition = new Vector3(targetTile.transform.position.x, transform.position.y, targetTile.transform.position.z);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
                tilesMoved++;
                actionPoints--;

                if (currentPathIndex == path.Count)
                {
                    if (actionPoints <= 0)
                    {
                        GameManager.Instance.currentCharacter.EndTurn();
                    }
                    else
                    {
                        tileSelected = false;
                        GameManager.Instance.currentCharacter.PlayTurn();
                    }
                    path.Clear();
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                Vector3 direction = (targetPosition - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }

    public virtual void PlayTurn()
    {
        isCurrentlyPlaying = true;
        currentTile = GetCurrentTile();
        reachableTiles = PathFindingTestTwo.FindReachableTiles(currentTile, actionPoints, tileSystem);
    }

    public virtual void EndTurn()
    {
        actionPoints = 0;
        isCurrentlyPlaying = false;
        if (!HasFinishedEvent())
        {
            Debug.Log($"{nameof(EndTurn)}: Can't end turn while dialogue is active.");
            StartCoroutine(WaitAndEndTurn());
            return;
        }
        Debug.Log("Ending turn.");
        ResetCharacter();
        GameManager.Instance.NextTurn();
    }

    private IEnumerator WaitAndEndTurn()
    {
        yield return new WaitUntil(() => HasFinishedPlaying());
        Debug.Log("Ending turn.");
        EndTurn();
    }

    public void ResetCharacter()
    {
        reachableTiles.Clear();
        currentPathIndex = 0;
        tilesMoved = 0;
        actionPoints = initialActionPoints;
    }

    public virtual int GetRemainingActionPoints()
    {
        return actionPoints - tilesMoved;
    }

    public virtual Tile GetCurrentTile()
    {
        return tileSystem.GetTileFromPosition(transform.position);
    }

    public void SwitchCamera()
    {
        CameraManager.instance.SwtichCamera(characterCam);
    }

    public bool HasFinishedPlaying()
    {
        return actionPoints == 0 && HasFinishedEvent();
    }

    public bool HasFinishedEvent()
    {
        Debug.Log($"{nameof(dialogueManager)}.{nameof(dialogueManager.Instance)}.{nameof(dialogueManager.Instance.isDialogueActive)}: {dialogueManager.Instance.isDialogueActive}");
        return dialogueManager.Instance.isDialogueActive == false;
    }
}

