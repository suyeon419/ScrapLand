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
    }
}

