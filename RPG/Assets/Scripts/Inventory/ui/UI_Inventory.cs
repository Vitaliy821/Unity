using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.AI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

class UI_Inventory : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject _inventoryHandler;
    [SerializeField] private GameObject _equipmentHandler;
    [SerializeField] private GameObject selected;

    private Inventory inventory;
    private List<InventoryItem> inventoryEquipment;
    private UI_Item[] items;
    private UI_Item[] equipments;

    private UI_Item selectedItem;
    private void Awake()
    {
        inventory = playerController.Inventory;
        inventoryEquipment = inventory.GetEquipment();

        items = _inventoryHandler.GetComponentsInChildren<ItemController>();
        equipments = _equipmentHandler.GetComponentsInChildren<EquipmentController>();

        SelectedItem img = selected.GetComponent<SelectedItem>();
        img.OnRightClick += OnRightClickListener;
        img.OnLeftClick += ChangePosition;


        img.OnDropItem += () =>
        {
            selectedItem.GetItem().Drop();
            selectedItem.SetItem(null);
            OnLeftClickListener(null);
        };

        img.OnLeftClickEquipment += (itemEvent) =>
        {
            if (itemEvent != null)
                if (((Equipment)selectedItem?.GetItem().Item).Type == itemEvent.EquipmentType)
                {
                    if ((itemEvent.GetItem() != null && inventory.ForceEquipe(selectedItem.GetItem()))
                        || (itemEvent.GetItem() != null && inventory.ForceEquipe(itemEvent.GetItem()))
                        || selectedItem.GetItem().Use())
                    {
                        DrawUI();
                        OnLeftClickListener(null);
                    }
                }
        };



        foreach (var item in equipments)
        {
            item.OnRightClick += (itemEvent) =>
            {
                if (itemEvent.Use())
                    DrawUI();
            };

            item.OnLeftClick += (itemEvent) =>
            {
                selectedItem = itemEvent;
                if (itemEvent == null)
                    selected.SetActive(false);

                if (itemEvent?.GetItem() != null)
                {
                    selected.SetActive(true);
                    selected.GetComponent<Image>().sprite = itemEvent.GetItem().Item.InventoryIcon;
                }
            };

            img.OnRightClick += item.BackgroundToDefault;
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Position = i;
            items[i].OnLeftClick += OnLeftClickListener;

            items[i].OnRightClick += (itemEvent) =>
            {
                if (itemEvent.Use())
                    DrawUI();
            };

            img.OnRightClick += items[i].BackgroundToDefault;
        }

    }
    private void ChangePosition(ItemController item)
    {
        if (item == null)
            return;

        if (item.GetItem() == null)
        {
            if (selectedItem is EquipmentController)
                inventory.ForceUnequipe(selectedItem.GetItem(), item.Position);

            item.SetItem(selectedItem.GetItem());
            selectedItem.SetItem(null);
            OnLeftClickListener(null);
        }
        else if (item.GetItem() == selectedItem.GetItem())
        {
            OnLeftClickListener(null);
            item.BackgroundToDefault();
        }
        else
        {
            if (selectedItem is EquipmentController)
                inventory.ForceUnequipe(selectedItem.GetItem(), item.Position);

            var objItem = item.GetItem();
            var selectedVar = selectedItem.GetItem();
            item.SetItem(null);
            selectedItem.SetItem(null);
            item.SetItem(selectedVar);
            selectedItem.SetItem(objItem, false);

            OnLeftClickListener(selectedItem);
        }
    }

    private void OnLeftClickListener(UI_Item item)
    {
        selectedItem = item;
        if (item == null)
            selected.SetActive(false);

        if (item?.GetItem() != null)
        {
            selected.SetActive(true);
            selected.GetComponent<Image>().sprite = item.GetItem().Item.InventoryIcon;
        }

    }

    private void OnRightClickListener()
    {
        if (selected != null)
        {
            OnLeftClickListener(null);
        }
    }

    private void Update()
    {
        if (selectedItem?.GetItem() != null)
            selected.transform.position = Input.mousePosition;
    }

    public void ChangeVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        OnLeftClickListener(null);

        if (gameObject.activeInHierarchy)
            DrawUI();
    }

    void DrawUI()
    {
        foreach (ItemController item in items)
        {
            item.SetItem(null);
        }

        foreach (InventoryItem item in inventory)
        {
            items[item.Position].SetItem(item);
        }

        foreach (EquipmentController item in equipments)
        {
            item.SetItem(null);
        }

        foreach (InventoryItem item in inventoryEquipment)
        {
            equipments.First(el => (el as EquipmentController).EquipmentType == ((Equipment)item.Item).Type).SetItem(item);
        }
    }

}

