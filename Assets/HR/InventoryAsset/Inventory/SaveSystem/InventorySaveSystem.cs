using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace InventorySystem
{
    //Author Jaxon Schauer
    /// <summary>
    /// Static class, takes in inventory data converts it into binary and places into a save location
    /// </summary>
    public static class InventorySaveSystem
    {
        public static void SaveInventory(Dictionary<string, Inventory> inventoryManager, string saveLocation)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveLocation.ToString();
            FileStream fileStream = new FileStream(path, FileMode.Create);
            InventoryData InventoryData = new InventoryData(inventoryManager);
            formatter.Serialize(fileStream, InventoryData);
            fileStream.Close();
        }
        public static InventoryData LoadItem(string saveLocation)
        {
            string path = Application.persistentDataPath + "/" + saveLocation.ToString();
            if (File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Open);
                if (fileStream.Length == 0)
                {
                    fileStream.Close();
                    return null;
                }
                BinaryFormatter formatter = new BinaryFormatter();
                InventoryData InventoryData = formatter.Deserialize(fileStream) as InventoryData;
                fileStream.Close();
                return InventoryData;
            }
            else
            {
                Debug.LogError("Save File " + path + " does not exist");
                return null;
            }
        }
        public static void Create(string saveLocation)
        {
            string path = Application.persistentDataPath + "/" + saveLocation.ToString();
            if (!File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Create);
                fileStream.Close();
            }
        }

        public static void Reset(string saveLocation)
        {
            string path = Application.persistentDataPath + "/" + saveLocation.ToString();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        //HH
        public static void SaveInventoryToJson(Dictionary<string, Inventory> inventoryManager, string fileName)
        {
            // 1. InventoryData ��ü ����
            InventoryData inventoryData = new InventoryData(inventoryManager);

            // 2. JSON ���ڿ��� ��ȯ
            string json = JsonUtility.ToJson(inventoryData, true);

            // 3. Assets ���� ��� ����
            string path = Path.Combine(Application.dataPath, fileName + ".json");

            // 4. ���Ϸ� ����
            File.WriteAllText(path, json);

            Debug.Log($"�κ��丮 �����Ͱ� JSON���� ����Ǿ����ϴ�: {path}");
        }

        public static void SaveInventoryWithBackup(Dictionary<string, Inventory> inventoryManager, string saveLocation, string backupFileName)
        {
            // 1. ���� ����
            SaveInventory(inventoryManager, saveLocation);

            // 2. ����(���) ���Ϸ� ����
            string originalPath = Application.persistentDataPath + "/" + saveLocation;
            string backupPath = Application.persistentDataPath + "/" + backupFileName;

            if (File.Exists(originalPath))
            {
                File.Copy(originalPath, backupPath, true); // true: ����� ���
                Debug.Log($"�κ��丮 ��� ���� ����: {backupPath}");
            }
            else
            {
                Debug.LogWarning("���� �κ��丮 ������ �������� �ʾ� ����� �����߽��ϴ�.");
            }
        }

        public static InventoryData LoadBackup(string backupFileName)
        {
            string backupPath = Application.persistentDataPath + "/" + backupFileName;
            if (File.Exists(backupPath))
            {
                FileStream fileStream = new FileStream(backupPath, FileMode.Open);
                if (fileStream.Length == 0)
                {
                    fileStream.Close();
                    return null;
                }
                BinaryFormatter formatter = new BinaryFormatter();
                InventoryData InventoryData = formatter.Deserialize(fileStream) as InventoryData;
                fileStream.Close();
                return InventoryData;
            }
            else
            {
                Debug.LogError("��� ������ �������� �ʽ��ϴ�: " + backupPath);
                return null;
            }
        }

        public static void RestoreInventoryFromBackup(InventoryData backupData)
        {
            if (backupData == null)
            {
                Debug.LogWarning("������ �����Ͱ� �����ϴ�.");
                return;
            }

            // ���� �κ��丮 �ʱ�ȭ
            foreach (var pair in InventoryController.instance.GetInventoryManager())
                pair.Value.Clear();

            // ��� �����ͷ� �κ��丮 ����
            foreach (var pair in backupData.inventories)
            {
                if (pair.Key == null) continue;
                if (!InventoryController.instance.GetInventoryManager().ContainsKey(pair.Key)) continue;

                Inventory inventory = InventoryController.instance.GetInventoryManager()[pair.Key];
                foreach (var item in pair.Value)
                {
                    if (item.name != null)
                    {
                        ItemInitializer itemInit = InventoryController.instance.GetItems().Find(x => x.GetItemType() == item.name);
                        if (itemInit != null)
                        {
                            InventoryItem template = new InventoryItem(itemInit);
                            InventoryItem newItem = new InventoryItem(template, item.amount);
                            inventory.AddItemPos(item.position, newItem);
                        }
                    }
                }
            }
            Debug.Log("��� �����ͷ� �κ��丮 ���� �Ϸ�");
        }
    }
}

