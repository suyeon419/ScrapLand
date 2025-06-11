using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;

public class InvenSaveTest : MonoBehaviour
{
    /*    [Header("========[ Inventory Setup ]========")]
        [SerializeField]
        public List<InventoryInitializer> initializeInventory = new List<InventoryInitializer>();

        public Dictionary<string, Inventory> inventories;
        public string saveFileName = "player_inventory.dat";*/

    public Button saveBtn;
    public Button loadBtn;

    public void SaveAllInventories()
    {
        // 1. 모든 인벤토리의 saveInventory를 true로 설정
        foreach (var pair in InventoryController.instance.GetInventoryManager())
        {
            pair.Value.SetSave(true);
        }

        // 2. 저장
        InventorySaveSystem.SaveInventory(
            InventoryController.instance.GetInventoryManager(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );

        // 3. 다시 false로 돌려놓기
        foreach (var pair in InventoryController.instance.GetInventoryManager())
        {
            pair.Value.SetSave(false);
        }
    }

    public void SaveAllInventoriesToCustomFile(string customFileName)
    {
        // 저장할 때만 saveInventory를 true로
        foreach (var pair in InventoryController.instance.GetInventoryManager())
            pair.Value.SetSave(true);

        InventorySaveSystem.SaveInventory(
            InventoryController.instance.GetInventoryManager(),
            customFileName // 예: "manual_save.dat"
        );

        foreach (var pair in InventoryController.instance.GetInventoryManager())
            pair.Value.SetSave(false);
    }

    public void LoadAllInventories()
    {
        /*        // 1. 모든 인벤토리의 saveInventory를 true로 설정
                foreach (var pair in InventoryController.instance.GetInventoryManager())
                {
                    pair.Value.SetSave(true);
                }

                // 2. 불러오기
                InventoryController.instance.LoadSave();

                // 3. 다시 false로 돌려놓기
                foreach (var pair in InventoryController.instance.GetInventoryManager())
                {
                    pair.Value.SetSave(false);
                }*/

        InventoryController.instance.LoadSave();

    }

/*    public void LoadAllInventoriesFromCustomFile(string customFileName)
    {
        InventoryData loadedData = InventorySaveSystem.LoadItem(customFileName);
        if (loadedData != null)
        {
            // 기존 인벤토리 초기화
            foreach (var pair in InventoryController.instance.GetInventoryManager())
                pair.Value.Clear();

            // 불러온 데이터로 인벤토리 복원
            foreach (var pair in loadedData.inventories)
            {
                if (pair.Key == null) continue;
                if (!InventoryController.instance.GetInventoryManager().ContainsKey(pair.Key)) continue;

                Inventory inventory = InventoryController.instance.GetInventoryManager()[pair.Key];
                foreach (var item in pair.Value)
                {
                    if (item.name != null)
                    {
                        InventoryItem copyItem = InventoryController.instance.GetItems().Find(x => x.GetItemType() == item.name);
                        if (copyItem != null)
                        {
                            InventoryItem newItem = new InventoryItem(copyItem, item.amount);
                            inventory.AddItemPos(item.position, newItem);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("저장된 인벤토리 데이터가 없습니다.");
        }
    }*/
}