using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class InventoryMissHandler : MonoBehaviour
{
    // Miss Action ¿ë
    public void RestoreItem(UnityEngine.Vector3 pos, InventorySystem.InventoryItem item)
    {
        InventorySystem.InventoryController.instance.AddItemPos(
            item.GetInventory(),
            item,
            item.GetPosition()
        );
    }

    // Miss Over Slot Action ¿ë
    public void RestoreItemOverSlot(InventorySystem.InventoryItem draggedItem, InventorySystem.InventoryItem inSlot)
    {
        InventorySystem.InventoryController.instance.AddItemPos(
            draggedItem.GetInventory(),
            draggedItem,
            draggedItem.GetPosition()
        );
    }
}
