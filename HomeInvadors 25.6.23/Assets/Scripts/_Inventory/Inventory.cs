using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory", order = 1)]
public class Inventory : ScriptableObject
{
    [Serializable]
    public class ItemInInventory
    {
        public Pickable pickable;
        public int amount;
        public Texture icon; // Field to store the item's icon

        public ItemInInventory(Pickable _pickable, int _amount) // Pickable _pickable)
        {
            pickable = _pickable;
            amount = _amount;
            icon = pickable.item.icon; // Grab icon from InventoryItem ScriptableObject
        }
    }

    public List<ItemInInventory> inventoryItems = new List<ItemInInventory>();

    public void AddItem(Pickable pickable)
    {
        InventoryItem item = pickable.item;

        int amount = item.amount;
        // Check if item already exists in the inventory
        ItemInInventory existingItem = inventoryItems.Find(i => i.pickable.item == item);

        if (existingItem != null)
        {
            // Increase the amount
            existingItem.amount += amount;
        }
        else
        {
            // Add new item to the inventory
            inventoryItems.Add(new ItemInInventory(pickable, amount));
        }
        Debug.Log(inventoryItems.Count);
    }

    public void RemoveItem(Pickable pickable)
    {
        InventoryItem item = pickable.item;
        ItemInInventory existingItem = inventoryItems.Find(i => i.pickable.item == item);

        if (existingItem != null)
        {
            existingItem.amount -= 1;
        }
        if (existingItem.amount == 0)
        {
            inventoryItems.Remove(existingItem);
        }
    }
}