using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public interface UI_Item : IPointerClickHandler
{
    int Position { get; set; }
    InventoryItem GetItem();
    void SetItem(InventoryItem item, bool updateBG = true);
    Action<UI_Item> OnLeftClick { get; set; }
    Action<InventoryItem> OnRightClick { get; set; }
    void BackgroundToDefault();
}

