using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager class is responsible for managing the overall game state,
/// including the turn-based system, dialogue system, and characters' inventory.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager.
    public static GameManager Instance;

    // References to various managers and systems.
    private dialogueManager _dialogueManager;
    public InventoryManager inventoryManager;
    private TurnManager _turnManager;
    public HUD Hud;

    // Reference to the current character whose turn it is.
    public Character currentCharacter;

    // Array of all characters in the game.
    public Character[] Characters;

    // Variables for controlling the turns.
    private float _turnDelayTimer = 2f;
    public bool isDialogueActive;
    public bool isCurrentlyPlaying;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// It initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// It initializes various game systems and managers.
    /// </summary>
    void Start()
    {
        _dialogueManager = dialogueManager.Instance;
        Characters = GameObject.FindObjectsOfType<Character>();
        _turnManager = new TurnManager(Characters);
        inventoryManager.InstantiateInventory(Characters);
        currentCharacter = Characters[0];

        StartNewTurn();
    }

    /// <summary>
    /// Update is called once per frame.
    /// It handles the game logic updates.
    /// </summary>
    void Update()
    {
        isCurrentlyPlaying = currentCharacter.isCurrentlyPlaying;

        currentCharacter = _turnManager.GetCurrentCharacter();
        isDialogueActive = dialogueManager.Instance.isDialogueActive;

        // Decrement the turn delay timer.
        if (_turnDelayTimer > 0f)
        {
            _turnDelayTimer -= Time.deltaTime;
        }
        else
        {
            // Control the current character and update HUD.
            currentCharacter.PlayTurn();
            Hud.UpdateActionPoints(currentCharacter.actionPoints);
        }
    }

    /// <summary>
    /// Starts a new turn for the current character.
    /// </summary>
    void StartNewTurn()
    {
        Character currentCharacter = _turnManager.GetCurrentCharacter();
        Hud.StartTurn(currentCharacter.characterName, _turnManager.GetRound());
        Hud.UpdateActionPoints(currentCharacter.actionPoints);
        inventoryManager.ChangeCurrentInventory(currentCharacter);

        _turnDelayTimer = 2f;
    }

    /// <summary>
    /// Ends the turn for the current character.
    /// </summary>
    public void EndTurnButton()
    {
        currentCharacter.EndTurn();
    }

    /// <summary>
    /// Advances to the next turn.
    /// </summary>
    public void NextTurn()
    {
        _turnManager.EndTurn();
        StartNewTurn();
    }
}

