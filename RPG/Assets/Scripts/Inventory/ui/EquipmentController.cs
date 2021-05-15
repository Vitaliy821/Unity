using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;

public class EquipmentController : MonoBehaviour, UI_Item
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite defaultBackgroundImage;
    public EquipmentType EquipmentType;
    public int Position { get; set; }
    private InventoryItem Item;
    public Action<UI_Item> OnLeftClick { get; set; }
    public Action<InventoryItem> OnRightClick { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == InputButton.Left)
        {
            OnLeftClick(this);
            backgroundImage.sprite = defaultBackgroundImage;
        }

        else if (eventData.button == InputButton.Right)
        {
            if (Item != null)
                OnRightClick(Item);
        }
    }

    public void SetItem(InventoryItem item, bool updateBG = true)
    {
        if (item == null)
        {
            Item = null;
            backgroundImage.sprite = defaultBackgroundImage;
            backgroundImage.color = new Color(255, 255, 255, 0.5f);
        }
        else
        {
            Item = item;
            item.Position = Position;

            backgroundImage.color = new Color(255, 255, 255, 1);
            if (updateBG)
            {
                backgroundImage.sprite = item.Item.InventoryIcon;

            }

        }
    }

    public void BackgroundToDefault()
    {
        if (Item != null)
            backgroundImage.sprite = Item.Item.InventoryIcon;
        else
            backgroundImage.sprite = defaultBackgroundImage;
    }

    public InventoryItem GetItem()
    {
        return Item;
    }
}

