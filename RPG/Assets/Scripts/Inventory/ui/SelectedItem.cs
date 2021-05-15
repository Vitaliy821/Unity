using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int dropAreaLayerNumber;
    public Action OnRightClick { get; set; }
    public Action OnDropItem { get; set; }
    public Action<ItemController> OnLeftClick { get; set; }
    public Action<EquipmentController> OnLeftClickEquipment { get; set; }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var result = Physics2D.Raycast(Input.mousePosition, new Vector3(0, 0, 1));
            ItemController item = result.collider?.gameObject.GetComponent<ItemController>();
            if (item != null)
                OnLeftClick(item);

            EquipmentController equipment = result.collider?.gameObject.GetComponent<EquipmentController>();
            if (equipment != null)
                OnLeftClickEquipment(equipment);

            if (item == null)
            {
                if (result.collider?.gameObject.layer == dropAreaLayerNumber)
                    OnDropItem();
            }
        }
    }
}
