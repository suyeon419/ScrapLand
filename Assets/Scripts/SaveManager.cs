using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SaveData
{
    public float brightnessValue;
    public float bgmVolume;
    public float sfxVolume;
    public float sensitivityValue;

    public int happyGage;
    public int coin;
    public int dayNum;

    public List<ItemUsageData> itemUsageList;
    public List<InteriorOnHouse> Interiors;
    public List<MachineData_save> Machines;

    public bool isBgmMuted;

    public List<bool> Completed;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    SaveData saveData = new();
    private string saveFilePath => Application.persistentDataPath + "/savefile.json";
    private string backupInven = "Inventory_Backup.dat";

    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        
        #endregion
    }

    public void SaveGame()
    {
        Debug.Log("세이브 시스템에서 저장#############");
        saveData.brightnessValue = GameManager_ScrapLand.instance.GetBrightness();
        saveData.bgmVolume = GameManager_ScrapLand.instance.GetBgmVolume();
        saveData.sfxVolume = GameManager_ScrapLand.instance.GetSfxVolume();
        saveData.sensitivityValue = GameManager_ScrapLand.instance.GetSensitivity();
        saveData.happyGage = GameManager_ScrapLand.instance.GetHappyGage();
        saveData.dayNum = GameManager_ScrapLand.instance.GetDayNum();
        saveData.coin = GameManager_ScrapLand.instance.GetCoin();

        saveData.itemUsageList = GameManager_ScrapLand.instance.GetItemUsageList();
        saveData.Interiors = GameManager_ScrapLand.instance.GetInteriorOnHouses();
        saveData.Machines = GameManager_ScrapLand.instance.GetMachineForSave();

        saveData.isBgmMuted = SoundManager.instance.GetBgmMuteStatus();

        saveData.Completed = GameManager_ScrapLand.instance.GetCompletedForSave().Values.ToList();

        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveFilePath, json);

        // 인벤 관련
        InventorySaveSystem.SaveInventoryWithBackup(
            InventoryController.instance.GetInventoryManager(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            backupInven);
    }


    public void LoadGame()
    {
        if (!System.IO.File.Exists(saveFilePath))
        {
            Debug.Log("세이브파일없어서 새로 만듦######");
            saveData = new SaveData();
            GameManager_ScrapLand.instance.ResetValues();
            return;
        }
        Debug.Log("세이브 시스템에서 로드#############");
        string json = System.IO.File.ReadAllText(saveFilePath);
        saveData = JsonUtility.FromJson<SaveData>(json);

        GameManager_ScrapLand.instance.SetBrightness(saveData.brightnessValue);
        GameManager_ScrapLand.instance.SetBgmVolume(saveData.bgmVolume);
        GameManager_ScrapLand.instance.SetSfxVolume(saveData.sfxVolume);
        GameManager_ScrapLand.instance.SetSensitivity(saveData.sensitivityValue);
        GameManager_ScrapLand.instance.SetHappyGage(saveData.happyGage);
        GameManager_ScrapLand.instance.SetDayNum(saveData.dayNum);
        GameManager_ScrapLand.instance.SetCoin(saveData.coin);

        GameManager_ScrapLand.instance.SetItemUsageList(saveData.itemUsageList);
        GameManager_ScrapLand.instance.SetInteriorList(saveData.Interiors);
        GameManager_ScrapLand.instance.SetMachineForSave(saveData.Machines);

        SoundManager.instance.SetBgmMuteStatus(saveData.isBgmMuted);

        var completedDict = GameManager_ScrapLand.instance.GetCompletedForSave();
        if (saveData.Completed != null && saveData.Completed.Count == completedDict.Count)
        {
            var keys = completedDict.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                completedDict[keys[i]] = saveData.Completed[i];
            }
            GameManager_ScrapLand.instance.SetCompletedForSave(completedDict);
        }

        if (GameManager_ScrapLand.instance.GetDayNum() > 1 && SceneManager.GetActiveScene().Equals("PlayScene"))
        {
            Debug.Log("인벤 데이터 복원합니다~ ##################");
            var backupData = InventorySaveSystem.LoadBackup(backupInven);
            InventorySaveSystem.RestoreInventoryFromBackup(backupData);
            PlayerInvenManager.instance.InvenClose();
        }
    }

    public void ResetGame()
    {
        Debug.Log("세이브 시스템에서 리셋#############");
        saveData = new SaveData();
        if(System.IO.File.Exists(saveFilePath))
        {
            System.IO.File.Delete(saveFilePath);
            GameManager_ScrapLand.instance.ResetValues();
        }
        if (System.IO.File.Exists(backupInven))
        {
            System.IO.File.Delete(Application.persistentDataPath + backupInven);
        }
        PlayerInvenManager.ResetSellCounts();
        InventorySaveSystem.Reset(SceneManager.GetActiveScene().name);
    }
}

