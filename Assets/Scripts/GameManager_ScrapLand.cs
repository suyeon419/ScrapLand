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

    public InteriorOnHouse(string name, Vector3 pos, Vector3 rot)
    {
        this.itemName = name;
        this.position = pos;
        this.rotation = rot;
    }
}

[System.Serializable]
public class MachineData_save
{
    public string machineName; // 이름
    public int hp; // 내구도 
    public bool activate; // 활성화
    public bool isPurchased; // 첫구매 유무

    public MachineData_save(string name, int hp)
    {
        this.machineName = name;
        this.hp = hp;
        this.activate = false;
        this.isPurchased = false;
    }
}


public class GameManager_ScrapLand : MonoBehaviour
{
    public static GameManager_ScrapLand instance;

    private float brightnessValue = 1;
    private float bgmVolume = 0.5f;
    private float sfxVolume = 0.5f;
    private float sensitivityValue = 0.3f;

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
        machines.Add(new MachineData_save("Filature", 10)); // 방적기
        machines.Add(new MachineData_save("Grinder", 45)); // 분쇄기
        machines.Add(new MachineData_save("BlastFurnace", 15)); // 용광로
        machines.Add(new MachineData_save("Compressor", 10)); // 압축기
        machines.Add(new MachineData_save("SewingMachine", 5)); // 재봉틀
    }

    /// <summery>
    /// 현재 저장되어있는 감도로 재 설정 하는 함수.(패널껐을때 불러오려고 만듦)
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
                Debug.LogWarning("Main Camera에 ThirdPersonCamera 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Main Camera 오브젝트를 찾을 수 없습니다.");

        }
    }

    #region 인테리어 관련
    public void New_Install_Interior(string itemName, Vector3 pos, Vector3 rot) // 맵에 설치했을 때 불러올 함수
    {
        Interiors.Add(new InteriorOnHouse(itemName, pos, rot));
    }

    public void Remove_Interior_AtPosition(Vector3 pos, Vector3 rot) // 인벤토리에 다시 집어넣을 때 불러올 함수
    {
        var target = Interiors.FirstOrDefault(i => i.position == pos && i.rotation == rot);
        if (target != null)
        {
            Interiors.Remove(target);
        }
    }

    public bool IsHappyGageAvailable_install(string itemName) // 인테리어 설치 횟수 확인해서 되면 +1
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
    #endregion


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
        Camera mainCam = Camera.main; // 태그가 MainCamera인 오브젝트 참조
        if (mainCam != null)
        {
            ThirdPersonCamera camera = mainCam.GetComponent<ThirdPersonCamera>();
            if (camera != null)
            {
                camera.SetSensitivity(value);
            }
            else
            {
                Debug.LogWarning("Main Camera에 ThirdPersonCamera 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Main Camera 오브젝트를 찾을 수 없습니다.");

        }
    }
    public void SetDayNum(int day)
    {
        Day_NUM = day;
    }
    public void SetCoin(int coin)
    {
        this.Coin = coin;
    }
    public void SetHappyGage(int value)
    {
        Debug.Log("저장된 게이지" + value);
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

    #endregion

    #region 기계 데이터
    /*Filature 방적기
    Grinder 분쇄기
    BlastFurnace 용광로
    Compressor 압축기
    SewingMachine 재봉틀
    */
    // isPurchased : 혜리, hp : 수연, activate : 혜리
    void MachineDataSend()
    {
        BlockController blockController = FindObjectOfType<BlockController>();
        if (blockController != null)
        {
            blockController.machine = GetMachine("Filature").isPurchased;
            blockController.breaker = GetMachine("Grinder").isPurchased;
            blockController.blastFurnace = GetMachine("BlastFurnace").isPurchased;
            blockController.compressor = GetMachine("Compressor").isPurchased;
        }
        SewingMachineController sewing = FindObjectOfType<SewingMachineController>();
        if (sewing != null)
        {
            sewing.SewingMachine = GetMachine("SewingMachine").hp;
        }
        Machine machine = FindObjectOfType<Machine>();
        if (machine != null)
        {
            machine.SetMachineDurability(GetMachine("Filature").hp);
            machine.SetBreakerDurability(GetMachine("Grinder").hp);
            machine.SetBlastFurnaceDurability(GetMachine("BlastFurnace").hp);
            machine.SetCompressorDurability(GetMachine("Compressor").hp);
        }

        if (ShopManager.Instance != null)
        {
            List<MachineData> m = ShopManager.Instance.machines;
            m[0].isPurchased = GetMachine("Grinder").isPurchased;
            m[1].isPurchased = GetMachine("SewingMachine").isPurchased;
            m[2].isPurchased = GetMachine("Filature").isPurchased;
            m[3].isPurchased = GetMachine("Compressor").isPurchased;
            m[4].isPurchased = GetMachine("BlastFurnace").isPurchased;

            m[0].isOnMap = GetMachine("Grinder").activate;
            m[1].isOnMap = GetMachine("SewingMachine").activate;
            m[2].isOnMap = GetMachine("Filature").activate;
            m[3].isOnMap = GetMachine("Compressor").activate;
            m[4].isOnMap = GetMachine("BlastFurnace").activate;
        }
    }
    public void SetHP_Machines(string name, int hp)
    {
        var machine = GetMachine(name);
        if (machine != null)
        {
            machine.hp = hp;
        }
    }
    public void SetActive_Machines(string name, bool active)
    {
        var machine = GetMachine(name);
        if (machine != null)
        {
            machine.activate = active;
        }
    }
    public void SetPurchased_Machines(string name, bool active)
    {
        var machine = GetMachine(name);
        if (machine != null)
        {
            machine.activate = active;
        }
    }
    public MachineData_save GetMachine(string name)
    {
        return machines.FirstOrDefault(m => m.machineName == name);
    }

    void SetMachineData()
    {
        if (ShopManager.Instance != null)
        {
            List<MachineData> m = ShopManager.Instance.machines;
            SetPurchased_Machines("Grinder", m[0].isPurchased);
            SetPurchased_Machines("SewingMachine", m[1].isPurchased);
            SetPurchased_Machines("Filature", m[2].isPurchased);
            SetPurchased_Machines("Compressor", m[3].isPurchased);
            SetPurchased_Machines("BlastFurnace", m[4].isPurchased);

            SetActive_Machines("Grinder", m[0].isOnMap);
            SetActive_Machines("SewingMachine", m[1].isOnMap);
            SetActive_Machines("Filature", m[2].isOnMap);
            SetActive_Machines("Compressor", m[3].isOnMap);
            SetActive_Machines("BlastFurnace", m[4].isOnMap);
        }

        Machine machine = FindObjectOfType<Machine>();
        if (machine != null)
        {
            machine.SetMachineDurability(GetMachine("Filature").hp);
            machine.SetBreakerDurability(GetMachine("Grinder").hp);
            machine.SetBlastFurnaceDurability(GetMachine("BlastFurnace").hp);
            machine.SetCompressorDurability(GetMachine("Compressor").hp);
        }
        SewingMachineController sewing = FindObjectOfType<SewingMachineController>();
        if (sewing != null) 
        {
            sewing.SewingMachine = GetMachine("SewingMachine").hp;
        }
    }
    #endregion

    #region 타 스크립트에 데이터 적용
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
            Debug.LogWarning("HappyEarth.instance가 null입니다!");
        }
    }

    public void ApplyCoinOnLoad()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.coin = Coin;
        }
        else
        {
            Debug.Log("CoinManager.instance가 null입니다.");
        }
    }
    #endregion

    #region 엔딩관련
    public void EndingChecking()
    {
        Debug.Log("엔딩조건: 모든 제품 제작, 게이지는 이미 검사 후 넘어온 것");
        if (CheckAllProduction())
        {
            SoundManager.instance.OnAndOffBGM(true);
            SaveManager.instance.ResetGame();
            SceneManager.LoadScene("HappyEndingScene");
        }
        else
        {
            SoundManager.instance.OnAndOffBGM(true);
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
    #endregion

    #region 저장관련

    public void SaveGame()
    {
        PlayerInvenManager.LoadSellCounts();
        SetMachineData();
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
        MachineDataSend();
        PlayerInvenManager.SaveSellCounts();
    }

    public void ResetValues()
    {
        SetHappyGage(-1);
        SetDayNum(1);
        SetCoin(0);
        InitializeItemUsages();
        InitializeMachines();
        Interiors.Clear();
        PlayerInvenManager.ResetSellCounts();
    }
    #endregion
}
