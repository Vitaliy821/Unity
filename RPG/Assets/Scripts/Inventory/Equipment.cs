using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Equipment", menuName = "Item/Equipment")]
public class Equipment : ItemBase
{
    [SerializeField] private EquipmentType _type;
    public EquipmentType Type => _type;
    private void Awake()
    {
        EditorUtility.SetDirty(this);
    }
}