using Controller;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ItemUsageData
{
    public string itemName;
    public int count_install;
    public int count_produce;

    public ItemUsageData() { }

    public ItemUsageData(string itemName)
    {
        this.itemName = itemName;
        this.count_install = 0;
        this.count_produce = 0;
    }
    public override string ToString()
    {
        return $"ItemUsageData(itemName: {itemName}, count_real: {count_install})";
    }
}

[System.Serializable]
public class InteriorOnHouse
{
    public string itemName;
    public Vector3 position;
    public Vector3 rotation;

    public InteriorOnHouse(string name,  Vector3 pos, Vector3 rot)
    {
        this.itemName = name;
        this.position = pos;
        this.rotation = rot;
    }
}

[System.Serializable]
public class MachineData_save
{
    public string machineName;
    public int hp;
    public bool activate;

    public MachineData_save(string name, int hp)
    {
        this.machineName = name;
        this.hp = hp;
        this.activate = false;
    }
}


public class GameManager_ScrapLand : MonoBehaviour
{
    public static GameManager_ScrapLand instance;

    private float brightnessValue = 1;
    private float bgmVolume = 0.5f;
    private float sfxVolume = 0.5f;
    private float sensitivityValue = 1f;

    private int HappyGage = -1;
    private int Coin = 0;

    private int Day_NUM = 1;

    private List<ItemUsageData> itemUsages = new List<ItemUsageData>();
    private List<InteriorOnHouse> Interiors = new List<InteriorOnHouse>();
    private List<MachineData_save> machines = new List<MachineData_save>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            InitializeItemUsages();
            InitializeMachines();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetSettings();
        LoadGame();
    }

    void SetSettings()
    {
        SetBgmVolume(bgmVolume);
        SetSfxVolume(sfxVolume);
        SetBrightness(brightnessValue);
        SetSensitivity(sensitivityValue);
    }

    void InitializeItemUsages()
    {
        itemUsages.Clear(); 
        itemUsages.Add(new ItemUsageData("Bag"));
        itemUsages.Add(new ItemUsageData("Hat"));
        itemUsages.Add(new ItemUsageData("Glove"));
        itemUsages.Add(new ItemUsageData("Shirt"));
        itemUsages.Add(new ItemUsageData("Pants"));
        itemUsages.Add(new ItemUsageData("Shoes"));
        itemUsages.Add(new ItemUsageData("Plastic Pot"));
        itemUsages.Add(new ItemUsageData("Can Pot"));
        itemUsages.Add(new ItemUsageData("Glass Pot"));
        itemUsages.Add(new ItemUsageData("Table"));
        itemUsages.Add(new ItemUsageData("Bench"));
        itemUsages.Add(new ItemUsageData("Old Chest"));
        itemUsages.Add(new ItemUsageData("Mobile"));
        itemUsages.Add(new ItemUsageData("Clock"));
        itemUsages.Add(new ItemUsageData("Keyring"));
        itemUsages.Add(new ItemUsageData("Tongs"));
        itemUsages.Add(new ItemUsageData("Cup"));
        itemUsages.Add(new ItemUsageData("Bowl"));
        itemUsages.Add(new ItemUsageData("Pet_Boat"));
        itemUsages.Add(new ItemUsageData("Bicycle"));
    }

    void InitializeMachines()
    {
        machines.Clear();
        machines.Add(new MachineData_save("Filature", 10)); // ������
        machines.Add(new MachineData_save("Grinder", 45)); // �м��
        machines.Add(new MachineData_save("BlastFurnace", 15)); // �뱤��
        machines.Add(new MachineData_save("Compressor", 10)); // �����
        machines.Add(new MachineData_save("SewingMachine", 5)); // ���Ʋ
    }



    public void New_Install_Interior(string itemName, Vector3 pos, Vector3 rot) // �ʿ� ��ġ���� �� �ҷ��� �Լ�
    {
        Interiors.Add(new InteriorOnHouse(itemName, pos, rot));
    }

    public void Remove_Interior_AtPosition(Vector3 pos, Vector3 rot) // �κ��丮�� �ٽ� ������� �� �ҷ��� �Լ�
    {
        var target = Interiors.FirstOrDefault(i => i.position == pos && i.rotation == rot);
        if (target != null)
        {
            Interiors.Remove(target);
        }
    }

    public bool IsHappyGageAvailable_install(string itemName) // ���׸��� ��ġ Ƚ�� Ȯ���ؼ� �Ǹ� +1
    {
        if (itemName == null) { return false; }

        ItemUsageData item = itemUsages.FirstOrDefault(item => item.itemName == itemName);

        if (item.count_install < 1)
        {
            item.count_install++;
            return true;
        }
        else
        {
            return false;
        }
    }


    #region Set
    public void SetBrightness(float value)
    {
        brightnessValue = value;

        var brightnessObj = FindObjectOfType<Brightness>();
        if (brightnessObj != null)
        {
            brightnessObj.AdjustBrightness(value);
        }
    }
    public void SetBgmVolume(float value)
    {
        bgmVolume = value;

        var sound = SoundManager.instance;
        if (sound != null)
        {
            sound.SetBgmVolume(value);
        }
    }
    public void SetSfxVolume(float value)
    {
        sfxVolume = value;

        var sound = SoundManager.instance;
        if (sound != null)
        {
            sound.SetSfxVolume(value);
        }
    }
    public void SetSensitivity(float value)
    {
        if (value <= 0) { value = 0.1f; }
        sensitivityValue = value;
        Camera mainCam = Camera.main; // �±װ� MainCamera�� ������Ʈ ����
        if (mainCam != null)
        {
            ThirdPersonCamera camera = mainCam.GetComponent<ThirdPersonCamera>();
            if (camera != null)
            {
                camera.SetSensitivity(value);
            }
            else
            {
                Debug.LogWarning("Main Camera�� ThirdPersonCamera ��ũ��Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Main Camera ������Ʈ�� ã�� �� �����ϴ�.");

        }
    }
    /// <summery>
    /// ���� ����Ǿ��ִ� ������ �� ���� �ϴ� �Լ�.(�гβ����� �ҷ������� ����)
    /// </summery>
    public void SetSensOrigin()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            ThirdPersonCamera camera = mainCam.GetComponent<ThirdPersonCamera>();
            if (camera != null)
            {
                camera.SetSensitivity(sensitivityValue);
            }
            else
            {
                Debug.LogWarning("Main Camera�� ThirdPersonCamera ��ũ��Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Main Camera ������Ʈ�� ã�� �� �����ϴ�.");

        }
    }
    public void SetDayNum(int day)
    {
        Day_NUM = day;
    }
    public void SetCoin(int coin)
    {
        Coin = coin;
    }
    public void SetHappyGage(int value)
    {
        Debug.Log("����� ������" + value);
        HappyGage = value;
    }
    public void SetItemUsageList(List<ItemUsageData> list)
    {
        if (list == null || list.Count == 0)
            return;

        itemUsages = list;
    }
    public void SetCount_Procude(string itemName, int value)
    {
        if (itemName == null) { return; }

        ItemUsageData item = itemUsages.FirstOrDefault(item => item.itemName == itemName);
        item.count_produce = value;
    }
    public void SetInteriorList(List<InteriorOnHouse> list)
    {
        if (list == null || list.Count == 0) return;

        Interiors = list;
    }
    public void SetMachineForSave(List<MachineData_save> m)
    {
        machines = m;
    }
    public void SetMachines(string name, int hp, bool activate)
    {
        var machine = GetMachine(name);
        if(machine != null)
        {
            machine.hp = hp;
            machine.activate = activate;
        }
    }
    public void SetActive(string name, bool active)
    {
        var machine = GetMachine(name);
        if (machine != null)
        {
            machine.activate = active;
        }
    }
    #endregion


    #region Get
    public float GetBrightness() { return brightnessValue; }
    public float GetBgmVolume() { return bgmVolume; }
    public float GetSfxVolume() { return sfxVolume; }
    public float GetSensitivity() { return sensitivityValue; }
    public int GetCoin() { return Coin; }
    public int GetHappyGage() { return HappyGage; }
    public int GetDayNum() { return Day_NUM; }
    public List<ItemUsageData> GetItemUsageList() { return itemUsages; }
    //public int GetCount_Sale(string itemName)
    //{
    //    ItemUsageData item = itemUsages.FirstOrDefault(item => item.itemName == itemName);
    //    return item.count_sale;
    //}
    public int GetCount_Produce(string itemName)
    {
        ItemUsageData item = itemUsages.FirstOrDefault(item => item.itemName == itemName);
        return item.count_produce;
    }
    public List<InteriorOnHouse> GetInteriorOnHouses() { return Interiors; }
    public List<MachineData_save> GetMachineForSave() { return machines; }
    public MachineData_save GetMachine(string name)
    {
        return machines.FirstOrDefault(m => m.machineName == name);
    }
    #endregion


    public void ApplyHappyEarthGageOnLoad()
    {
        if (HappyEarth.instance != null)
        {
            if (HappyGage == -1)
            {
                HappyGage = 0;
            }
            HappyEarth.instance.setRealGage(HappyGage);
        }
        else
        {
            Debug.LogWarning("HappyEarth.instance�� null�Դϴ�!");
        }
    }

    public void ApplyCoinOnLoad()
    {
        // ���� �� ���� ��ũ��Ʈ�� ��ȣ�ۿ��� ����
    }


    public void EndingChecking()
    {
        Debug.Log("��������: ��� ��ǰ ����, �������� �̹� �˻� �� �Ѿ�� ��");
        if (CheckAllProduction())
        {
            SoundManager.instance.TurnOff_BGM();
            SaveManager.instance.ResetGame();
            SceneManager.LoadScene("HappyEndingScene");
        }
        else
        {
            SoundManager.instance.TurnOff_BGM();
            SaveManager.instance.ResetGame();
            SceneManager.LoadScene("BadEndingScene");
        }
    }

    private bool CheckAllProduction()
    {
        foreach (var item in itemUsages)
        {
            if (item.count_produce > 0) continue;
            else return false;
        }
        return true;
    }

    public void SaveGame()
    {
        SaveManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        if (itemUsages == null || machines == null || itemUsages.Count == 0 || machines.Count == 0)
        {
            InitializeMachines();
            InitializeItemUsages();
            Interiors.Clear();
        }
        SaveManager.instance.LoadGame();
    }

    public void ResetValues()
    {
        SetHappyGage(-1);
        SetDayNum(1);
        InitializeItemUsages();
        InitializeMachines();
        Interiors.Clear();
    }

}
