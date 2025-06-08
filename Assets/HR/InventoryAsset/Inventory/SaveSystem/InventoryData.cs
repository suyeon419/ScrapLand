using System.Collections.Generic;
namespace InventorySystem
{
    //Author Jaxon Schauer
    /// <summary>
    /// creates a dictionary that holds all information related to the inventories.
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        public Dictionary<string, List<ItemData>> inventories;
        //이게 전체 인벤토리의 위치 저장하는 어쩌구

        public string itemType;
        public InventoryData(Dictionary<string, Inventory> inventoryManager)
        {
            inventories = new Dictionary<string, List<ItemData>>();
            foreach (var pair in inventoryManager)
            {
                int position = 0;
                if (!inventoryManager[pair.Key].GetSaveInventory())
                {
                    continue;
                }
                List<ItemData> itemData = new List<ItemData>();
                Inventory inventory = pair.Value;
                foreach(InventoryItem item in inventory.GetList())
                {
                    itemData.Add(new ItemData(item.GetAmount(),item.GetItemType(), position));
                    position++;
                }
                inventories.Add(inventory.GetName(), itemData);
            }
        }
    }
}
