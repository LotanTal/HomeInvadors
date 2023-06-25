using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventoryGridElementPrefab;
    [SerializeField] private Transform itemsGridContainer;
    [SerializeField] public Inventory inventory; // Assign the inventory ScriptableObject in the inspector

    public int maxItemsInInventory = 5;
    public Dictionary<Pickable, GameObject> inventoryItemElements = new Dictionary<Pickable, GameObject>();

    public void UpdateInventoryUI(Inventory inventory, Pickable pickable)
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is null.");
            return;
        }

        if (inventoryGridElementPrefab == null)
        {
            Debug.LogError("Inventory Grid Element Prefab is not assigned in the Unity Editor.");
            return;
        }

        if (itemsGridContainer == null)
        {
            Debug.LogError("Items Grid Container is not assigned in the Unity Editor.");
            return;
        }

        Debug.LogFormat("Update Inventory with {0} items", inventory.inventoryItems.Count);

        // Remove old inventory elements
        foreach (var element in inventoryItemElements)
        {
            Destroy(element.Value);
        }
        inventoryItemElements.Clear();

        // Loop through each item in the inventory
        foreach (var inventoryItem in inventory.inventoryItems)
        {
            Pickable item = inventoryItem.pickable;

            // Check if the item already exists in the dictionary
            if (inventoryItemElements.ContainsKey(item))
            {
                // Update the existing button with new data
                GameObject uiElement = inventoryItemElements[item];
                InventoryButton button = uiElement.GetComponent<InventoryButton>();
                button.UpdateButtonData(item.name, inventoryItem.amount, inventoryItem.icon, item);
            }
            else
            {
                // Create a new button for the item
                GameObject uiElement = Instantiate(inventoryGridElementPrefab, itemsGridContainer);
                InventoryButton button = uiElement.GetComponent<InventoryButton>();

                if (button == null)
                {
                    Debug.LogError("InventoryButton component is not attached to the Inventory Grid Element Prefab.");
                    return;
                }

                button.UpdateButtonData(item.name, inventoryItem.amount, inventoryItem.icon, item);

                inventoryItemElements.Add(item, uiElement);
            }
        }
    }
}