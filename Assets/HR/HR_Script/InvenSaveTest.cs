using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class InvenSaveTest : MonoBehaviour
{
/*    [Header("========[ Inventory Setup ]========")]
    [SerializeField]
    public List<InventoryInitializer> initializeInventory = new List<InventoryInitializer>();

    public Dictionary<string, Inventory> inventories;
    public string saveFileName = "player_inventory.dat";*/

    void Awake()
    {
        /*        inventories = new Dictionary<string, Inventory>();

                foreach (var initializer in initializeInventory)
                {
                    string id = initializer.GetInventoryName();
                    Inventory inventory = initializer.GetInventoryRef();

                    if (!string.IsNullOrEmpty(id) && inventory != null)
                    {
                        inventories[id] = inventory;
                    }
                    else
                    {
                        Debug.LogWarning("Invalid InventoryInitializer: missing name or reference.");
                    }
                }*/
/*        foreach (var pair in InventoryController.instance.GetInventoryManager())
        {
            Debug.Log($"[SaveInventory] Key: '{pair.Key}', Inventory: {pair.Value}");
        }*/
    }

    //Dictionary<string, Inventory> inventoryManager = new Dictionary<string, Inventory>();
    //InventoryUIManager["PlayerInven"] = HotBar;



    public void EndDay()
    {
        /*        if (inventories == null)
                {
                    Debug.LogError("Inventories is null. Cannot save!");
                    return;
                }

                // 인벤토리 저장
                InventorySaveSystem.SaveInventory(inventories, saveFileName);

                Debug.Log("Inventory saved at the end of the day.");*/
        /*        foreach (var kvp in inventories)
                {
                    kvp.Value.SetSave(true); // 하루가 끝날 때만 저장 활성화
                }

                Debug.Log("Inventory marked for save at the end of the day.");*/
        string filePath = System.IO.Path.Combine(Application.persistentDataPath);
        InventorySaveSystem.SaveInventory(InventoryController.instance.GetInventoryManager(), filePath);
    }
}