using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory startingInventory;
    public Inventory currentInventory;
    public List<Inventory> inventories = new List<Inventory>();
    public InventoryPanel inventoryPanel;

    public void InstantiateInventory(Character[] characters)
    {
        foreach (Character character in characters)
        {
            Inventory inventory = Instantiate(startingInventory);
            inventory.name = character.name + "'s inventory";
            inventories.Add(inventory);
            // Assign the inventory to the corresponding character
            character.inventory = inventory;
            Debug.Log(character.name + " has " + inventory.name);
        }
    }
    public void ChangeCurrentInventory(Character currentCharacter)
    {
        currentInventory = currentCharacter.inventory;
        inventoryPanel.inventory = currentInventory;
    }
    public void AddItem(Pickable pickable)
    {
        currentInventory.AddItem(pickable);
        inventoryPanel.UpdateInventoryUI(currentInventory, pickable);
    }
    public void RemoveItem(Pickable pickable)
    {
        currentInventory.RemoveItem(pickable);
        inventoryPanel.UpdateInventoryUI(currentInventory, pickable);
    }
}
