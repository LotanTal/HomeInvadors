using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/InventoryItem", order = 0)]
public class InventoryItem : ScriptableObject
{
    public new string name;
    public string description;
    public GameObject prefab;
    public Texture icon;
    public int amount;
}
