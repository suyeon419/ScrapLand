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
        // 1. ��� �κ��丮�� saveInventory�� true�� ����
        foreach (var pair in InventoryController.instance.GetInventoryManager())
        {
            pair.Value.SetSave(true);
        }

        // 2. ����
        InventorySaveSystem.SaveInventory(
            InventoryController.instance.GetInventoryManager(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );

        // 3. �ٽ� false�� ��������
        foreach (var pair in InventoryController.instance.GetInventoryManager())
        {
            pair.Value.SetSave(false);
        }
    }

    public void SaveAllInventoriesToCustomFile(string customFileName)
    {
        // ������ ���� saveInventory�� true��
        foreach (var pair in InventoryController.instance.GetInventoryManager())
            pair.Value.SetSave(true);

        InventorySaveSystem.SaveInventory(
            InventoryController.instance.GetInventoryManager(),
            customFileName // ��: "manual_save.dat"
        );

        foreach (var pair in InventoryController.instance.GetInventoryManager())
            pair.Value.SetSave(false);
    }

    public void LoadAllInventories()
    {
        /*        // 1. ��� �κ��丮�� saveInventory�� true�� ����
                foreach (var pair in InventoryController.instance.GetInventoryManager())
                {
                    pair.Value.SetSave(true);
                }

                // 2. �ҷ�����
                InventoryController.instance.LoadSave();

                // 3. �ٽ� false�� ��������
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
            // ���� �κ��丮 �ʱ�ȭ
            foreach (var pair in InventoryController.instance.GetInventoryManager())
                pair.Value.Clear();

            // �ҷ��� �����ͷ� �κ��丮 ����
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
            Debug.Log("����� �κ��丮 �����Ͱ� �����ϴ�.");
        }
    }*/
}