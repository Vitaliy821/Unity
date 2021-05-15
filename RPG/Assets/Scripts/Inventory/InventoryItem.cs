using System;
using UnityEngine;


[Serializable]
public class InventoryItem
{
    [SerializeField] private ItemBase _item;
    public int Position { get; set; }
    public ItemBase Item => _item;
    public Inventory Inventory { get; set; }

    public bool Use()
    {
        return Inventory.EquipeItem(this);
    }

    public void Drop()
    {
        Inventory.RemoveItem(this);
    }
}

