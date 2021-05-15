using UnityEditor;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] private ItemId _itemId;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private int _stackCount;
    [SerializeField] private Sprite _inventoryIcon;

    public ItemId ItemId => _itemId;
    public string Name => _name;
    public string Description => _description;
    public int Cost => _cost;
    public int StackCount => _stackCount;
    public Sprite InventoryIcon => _inventoryIcon;
}

