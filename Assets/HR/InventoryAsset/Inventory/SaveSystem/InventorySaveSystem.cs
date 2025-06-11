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
            // 1. InventoryData 객체 생성
            InventoryData inventoryData = new InventoryData(inventoryManager);

            // 2. JSON 문자열로 변환
            string json = JsonUtility.ToJson(inventoryData, true);

            // 3. Assets 폴더 경로 생성
            string path = Path.Combine(Application.dataPath, fileName + ".json");

            // 4. 파일로 저장
            File.WriteAllText(path, json);

            Debug.Log($"인벤토리 데이터가 JSON으로 저장되었습니다: {path}");
        }

        public static void SaveInventoryWithBackup(Dictionary<string, Inventory> inventoryManager, string saveLocation, string backupFileName)
        {
            // 1. 원본 저장
            SaveInventory(inventoryManager, saveLocation);

            // 2. 복제(백업) 파일로 복사
            string originalPath = Application.persistentDataPath + "/" + saveLocation;
            string backupPath = Application.persistentDataPath + "/" + backupFileName;

            if (File.Exists(originalPath))
            {
                File.Copy(originalPath, backupPath, true); // true: 덮어쓰기 허용
                Debug.Log($"인벤토리 백업 파일 생성: {backupPath}");
            }
            else
            {
                Debug.LogWarning("원본 인벤토리 파일이 존재하지 않아 백업에 실패했습니다.");
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
                Debug.LogError("백업 파일이 존재하지 않습니다: " + backupPath);
                return null;
            }
        }

        public static void RestoreInventoryFromBackup(InventoryData backupData)
        {
            if (backupData == null)
            {
                Debug.LogWarning("복원할 데이터가 없습니다.");
                return;
            }

            // 기존 인벤토리 초기화
            foreach (var pair in InventoryController.instance.GetInventoryManager())
                pair.Value.Clear();

            // 백업 데이터로 인벤토리 복원
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
            Debug.Log("백업 데이터로 인벤토리 복원 완료");
        }
    }
}

