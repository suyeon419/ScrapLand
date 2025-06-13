using InventorySystem;
using System.Collections;
using System.Collections.Generic;
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

    public Dictionary<string, bool> Completed;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    SaveData saveData = new();
    private string saveFilePath => Application.persistentDataPath + "/savefile.json";
    private string backupInven = "Inventory_Backup.dat";

    private void Awake()
    {
        #region ½Ì±ÛÅæ
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
    }

    public void SaveGame()
    {
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

        saveData.Completed = GameManager_ScrapLand.instance.GetCompletedForSave();

        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveFilePath, json);

        // ÀÎº¥ °ü·Ã
        InventorySaveSystem.SaveInventoryWithBackup(
            InventoryController.instance.GetInventoryManager(),
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            backupInven);
    }


    public void LoadGame()
    {
        if (!System.IO.File.Exists(saveFilePath))
        {
            saveData = new SaveData();
            GameManager_ScrapLand.instance.ResetValues();
            return;
        }

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

        GameManager_ScrapLand.instance.SetCompletedForSave(saveData.Completed);

        if (GameManager_ScrapLand.instance.GetDayNum() > 0)
        {
            var backupData = InventorySaveSystem.LoadBackup(backupInven);
            InventorySaveSystem.RestoreInventoryFromBackup(backupData);
            PlayerInvenManager.instance.InvenClose();
        }
    }

    public void ResetGame()
    {
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

