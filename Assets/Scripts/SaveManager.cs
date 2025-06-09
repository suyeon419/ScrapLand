using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


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
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    SaveData saveData = new();
    private string saveFilePath => Application.persistentDataPath + "/savefile.json";

    private void Awake()
    {
        #region ΩÃ±€≈Ê
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

        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveFilePath, json);
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
    }

    public void ResetGame()
    {
        saveData = new SaveData();
        if(System.IO.File.Exists(saveFilePath))
        {
            System.IO.File.Delete(saveFilePath);
            GameManager_ScrapLand.instance.ResetValues();
        }
    }
}

