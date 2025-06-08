using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : MonoBehaviour
{
    public List<ItemData> items;

    private Dictionary<string, int> priceMap;

    private void OnEnable()
    {
        priceMap = new Dictionary<string, int>();
        foreach (var item in items)
        {
            priceMap[item.itemType] = item.price;
        }
    }

    public int GetPrice(string itemType)
    {
        if (priceMap.TryGetValue(itemType, out int price))
            return price;
        else
            return 0;
    }
}

[System.Serializable]
public class ItemData
{
    public string itemType;
    public int price;

    public ItemData(string type, int price)
    {
        this.itemType = type;
        this.price = price;
    }
}
