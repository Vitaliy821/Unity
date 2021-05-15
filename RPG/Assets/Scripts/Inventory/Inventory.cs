using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IEnumerable<InventoryItem>
{
    private int size;
    private readonly List<InventoryItem> backpack;
    private readonly List<InventoryItem> equipments;

    public Inventory(int capacity)
    {
        backpack = new List<InventoryItem>();
        size = capacity;
        equipments = new List<InventoryItem>();
    }

    internal bool EquipeItem(InventoryItem item)
    {
        if (item == null)
            return false;
        if (!(item.Item is Equipment))
            return false;

        var itemEq = equipments.Find(el => ((Equipment)el.Item).Type == (item.Item as Equipment).Type);

        if (itemEq == null)
        {
            equipments.Add(item);
            backpack.Remove(item);

            return true;
        }
        else if (itemEq != item)
        {
            equipments.Remove(itemEq);
            equipments.Add(item);
            backpack.Remove(item);
            itemEq.Position = ChoosePosition();
            backpack.Add(itemEq);


            return true;
        }
        else
        {
            equipments.Remove(item);
            item.Position = ChoosePosition();
            backpack.Add(item);

            return true;
        }
    }

    internal List<InventoryItem> GetEquipment()
    {
        return equipments;
    }

    public bool AddItem(InventoryItem item)
    {
        if (backpack.Count >= size)
            return false;

        item.Inventory = this;
        item.Position = ChoosePosition();
        backpack.Add(item);

        return true;
    }

    private int ChoosePosition()
    {
        int position = 0;

        while (position < backpack.Count)
        {
            int pos = 1;
            foreach (var item in backpack)
            {
                if (item.Position == position)
                    break;
                pos++;
            }

            if (pos > backpack.Count)
                return position;

            position++;
        }
        return position;

    }

    internal bool ForceEquipe(InventoryItem item)
    {
        if (!(item.Item is Equipment))
            return false;

        if (equipments.Contains(item))
            return true;
        else
            return EquipeItem(item);
    }

    internal bool ForceUnequipe(InventoryItem item, int position = -1)
    {
        if (!(item.Item is Equipment))
            return false;

        if (equipments.Contains(item))
        {
            equipments.Remove(item);

            if (position == -1)
                position = ChoosePosition();

            item.Position = position;
            backpack.Add(item);

            return true;
        }
        else
            return false;
    }

    public void RemoveItem(InventoryItem item)
    {
        backpack.Remove(item);
        equipments.Remove(item);
        item.Inventory = null;
        Debug.Log("Dropped");
    }

    public IEnumerator<InventoryItem> GetEnumerator()
    {
        return backpack.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return backpack.GetEnumerator();
    }
}

