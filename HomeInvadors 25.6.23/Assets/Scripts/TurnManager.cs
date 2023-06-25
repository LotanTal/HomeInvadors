using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
///  This class manages the turn of the characters.
///  It also keeps track of the current round.
///  It also switches the camera of the characters.
///  It also keeps track of the current character.
///  It also keeps track of the current round.
/// </summary>

public class TurnManager
{
    private Character[] characters;
    private int currentRound = 0;
    private int currentCharacter = 0;

    public TurnManager(Character[] characters)
    {
        this.characters = characters;
    }

    // Ends the turn of the current character and switches to the next character or round.
    public void EndTurn()
    {
        if (currentCharacter < characters.Length - 1)
        {

            currentCharacter++;   // If there are more characters remaining, move to the next character
        }
        else
        {

            currentRound++;       // If all characters have completed their turns, move to the next round
            currentCharacter = 0; // Reset to the first character
        }


        characters[currentCharacter].SwitchCamera();  // Switch camera to the current character
    }

    // Retrieves the current character in the turn order.
    public Character GetCurrentCharacter()
    {
        return characters[currentCharacter];
    }

    // Retrieves the current round number.
    public int GetRound()
    {
        return currentRound;
    }
}