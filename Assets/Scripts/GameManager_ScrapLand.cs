using Controller;
using JetBrains.Annotations;
using OpenCover.Framework.Model;
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
    public string machineName; // �̸�
    public int hp; // ������ 
    public bool activate; // Ȱ��ȭ
    public bool isPurchased; // ù���� ����

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
    private Dictionary<string, int> ingredient = new Dictionary<string, int>();

    private Dictionary<string, bool> Completed = new Dictionary<string, bool>();

    private int[] passing_check = { 0, 50, 100, 200, 400, 650, 900 };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            InitializeItemUsages();
            InitializeMachines();
            InitializeCompleted();
            InitializeIngredient();
        }
        else if (instance != this)
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
        SoundManager.instance.PlayBGM("Main");

        if (SceneManager.GetActiveScene().name.Equals("PlayScene"))
        {
            //Debug.Log("�÷��̾����� �·ε�");
            SetSettings();
            LoadGame();
        }
        else
        {
            //Debug.Log("�÷��̾� XXXXXXXX �·ε�");
            SaveManager.instance.LoadGame();
        }
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
        itemUsages.Add(new ItemUsageData("Doll"));
        itemUsages.Add(new ItemUsageData("Pet_Boat"));
        #region ������ ��ǰ
        itemUsages.Add(new ItemUsageData("Bicycle_Frame"));
        itemUsages.Add(new ItemUsageData("Bicycle_Wheel"));
        itemUsages.Add(new ItemUsageData("Bicycle_Chain"));
        itemUsages.Add(new ItemUsageData("Bicycle_Handle"));
        itemUsages.Add(new ItemUsageData("Bicycle_Brake"));
        itemUsages.Add(new ItemUsageData("Bicycle_Saddle"));
        #endregion
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

    void InitializeCompleted()
    {
        Completed.Clear();
        Completed.Add("ptthread", false);
        Completed.Add("glass_break", false);
        Completed.Add("plastic_break", false);
        Completed.Add("can_break", false);
        Completed.Add("glass_molten", false);
        Completed.Add("plastic_molten", false);
        Completed.Add("can_molten", false);
        Completed.Add("compressed_paper", false);
    }

    void InitializeIngredient()
    {
        ingredient.Clear();
        ingredient.Add("pt_deleteCount", 0);
        ingredient.Add("glass_deleteCount", 0);
        ingredient.Add("plastic_deleteCount", 0);
        ingredient.Add("can_deleteCount", 0);
        ingredient.Add("b_glass_deleteCount", 0);
        ingredient.Add("b_plastic_deleteCount", 0);
        ingredient.Add("b_can_deleteCount", 0);
        ingredient.Add("paper_deleteCount", 0);
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

    #region ���׸��� ����
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

    public bool IsHappyGageAvailable_install(string itemName)
    {
        if (itemName == null) { return false; }

        ItemUsageData item = itemUsages.FirstOrDefault(item => item.itemName == itemName);

        if (item == null) return false; //itemUsages�� ���� ��� : (1) �̸��� �߸��Ǿ��ų�, (2) ���ų�

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
    void ReLoadInterior()
    {
        foreach (var interior in Interiors)
        {
            PlacementManager.Instance.ReLoadItem(interior.itemName, interior.position, interior.rotation);
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
    public void SetCompletedForSave(Dictionary<string, bool> com)
    {
        Completed = com;
    }
    public void SetingredientForSave (Dictionary<string, int> ing)
    {
        ingredient = ing;
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
    public Dictionary<string, bool> GetCompletedForSave() { return Completed; }
    public Dictionary<string, int> GetingredientForSave() { return ingredient; }
    #endregion

    #region ��� ������
    /*Filature ������
    Grinder �м��
    BlastFurnace �뱤��
    Compressor �����
    SewingMachine ���Ʋ
    */
    // isPurchased : ����, hp : ����, activate : ����
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
            sewing.CapMaking = GetCount_Produce("Hat");
            sewing.GroveMaking = GetCount_Produce("Glove");
            sewing.TopMaking = GetCount_Produce("Shirt");
            sewing.BottomMaking = GetCount_Produce("Pants");
            sewing.ShoesMaking = GetCount_Produce("Shoes");
            sewing.DollMaking = GetCount_Produce("Doll");
        }
        MakingController making = FindObjectOfType<MakingController>();
        if (making != null)
        {
            making.BagMaking = GetCount_Produce("Bag");
            making.P_PotMaiking = GetCount_Produce("Plastic Pot");
            making.C_PotMaiking = GetCount_Produce("Can Pot");
            making.G_PotMaiking = GetCount_Produce("Glass Pot");
            making.TableMaking = GetCount_Produce("Table");
            making.ChairMaking = GetCount_Produce("Bench");
            making.StorageBoxMaking = GetCount_Produce("Old Chest");
            making.MobileMaking = GetCount_Produce("Mobile");
            making.ClockMaking = GetCount_Produce("Clock");
            making.KeyringMaking = GetCount_Produce("Keyring");
            making.TongsMaking = GetCount_Produce("Tongs");
            making.CupMaking = GetCount_Produce("Cup");
            making.BowlMaking = GetCount_Produce("Bowl");
            making.BoatMaking = GetCount_Produce("Pet_Boat");
            making.FrameMaking = GetCount_Produce("Bicycle_Frame");
            making.WheelMaking = GetCount_Produce("Bicycle_Wheel");
            making.ChainMaking = GetCount_Produce("Bicycle_Chain");
            making.HandleMaking = GetCount_Produce("Bicycle_Handle");
            making.BrakeMaking = GetCount_Produce("Bicycle_Brake");
            making.SaddleMaking = GetCount_Produce("Bicycle_Saddle");
            making.BikeMaking = GetCount_Produce("Bicycle");
        }
        Machine machine = FindObjectOfType<Machine>();
        if (machine != null)
        {
            machine.SetMachineDurability(GetMachine("Filature").hp);
            machine.SetBreakerDurability(GetMachine("Grinder").hp);
            machine.SetBlastFurnaceDurability(GetMachine("BlastFurnace").hp);
            machine.SetCompressorDurability(GetMachine("Compressor").hp);

            machine.SetPtThreadCompletion(Completed["ptthread"]);
            machine.SetGlassBreakCompletion(Completed["glass_break"]);
            machine.SetPlasticBreakCompletion(Completed["plastic_break"]);
            machine.SetCanBreakCompletion(Completed["can_break"]);
            machine.SetGlassMoltenCompletion(Completed["glass_molten"]);
            machine.SetPlasticMoltenCompletion(Completed["plastic_molten"]);
            machine.SetCanMoltenCompletion(Completed["can_molten"]);
            machine.SetCompressedPaperCompletion(Completed["compressed_paper"]);

            machine.SetPtDeleteCount(ingredient["pt_deleteCount"]);
            machine.SetGlassDeleteCount(ingredient["glass_deleteCount"]);
            machine.SetPlasticDeleteCount(ingredient["plastic_deleteCount"]);
            machine.SetCanDeleteCount(ingredient["can_deleteCount"]);
            machine.SetBGlassDeleteCount(ingredient["b_glass_deleteCount"]);
            machine.SetBPlasticDeleteCount(ingredient["b_plastic_deleteCount"]);
            machine.SetBCanDeleteCount(ingredient["b_can_deleteCount"]);
            machine.SetPaperDeleteCount(ingredient["paper_deleteCount"]);
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
    public void SetPurchased_Machines(string name, bool purchased)
    {
        var machine = GetMachine(name);
        if (machine != null)
        {
            machine.isPurchased = purchased;
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
            SetHP_Machines("Filature", machine.GetMachineDurability());
            SetHP_Machines("Grinder", machine.GetBreakerDurability());
            SetHP_Machines("BlastFurnace", machine.GetBlastFurnaceDurability());
            SetHP_Machines("Compressor", machine.GetCompressorDurability());

            Completed["ptthread"] = machine.IsPtThreadCompleted();
            Completed["glass_break"] = machine.IsGlassBreakCompleted();
            Completed["plastic_break"] = machine.IsPlasticBreakCompleted();
            Completed["can_break"] = machine.IsCanBreakCompleted();
            Completed["glass_molten"] = machine.IsGlassMoltenCompleted();
            Completed["plastic_molten"] = machine.IsPlasticMoltenCompleted();
            Completed["can_molten"] = machine.IsCanMoltenCompleted();
            Completed["compressed_paper"] = machine.IsCompressedPaperCompleted();

            ingredient["pt_deleteCount"] = machine.GetPtDeleteCount();
            ingredient["glass_deleteCount"] = machine.GetGlassDeleteCount();
            ingredient["plastic_deleteCount"] = machine.GetPlasticDeleteCount();
            ingredient["can_deleteCount"] = machine.GetCanDeleteCount();
            ingredient["b_glass_deleteCount"] = machine.GetBGlassDeleteCount();
            ingredient["b_plastic_deleteCount"] = machine.GetBPlasticDeleteCount();
            ingredient["b_can_deleteCount"] = machine.GetBCanDeleteCount();
            ingredient["paper_deleteCount"] = machine.GetPaperDeleteCount();
        }
        MakingController making = FindObjectOfType<MakingController>();
        if(making != null)
        {
            SetCount_Procude("Bag", making.BagMaking);
            SetCount_Procude("Plastic Pot", making.P_PotMaiking);
            SetCount_Procude("Can Pot", making.C_PotMaiking);
            SetCount_Procude("Glass Pot", making.G_PotMaiking);
            SetCount_Procude("Table", making.TableMaking);
            SetCount_Procude("Bench", making.ChairMaking);
            SetCount_Procude("Old Chest", making.StorageBoxMaking);
            SetCount_Procude("Mobile", making.MobileMaking);
            SetCount_Procude("Clock", making.ClockMaking);
            SetCount_Procude("Keyring", making.KeyringMaking);
            SetCount_Procude("Tongs", making.TongsMaking);
            SetCount_Procude("Cup", making.CupMaking);
            SetCount_Procude("Bowl", making.BowlMaking);
            SetCount_Procude("Pet_Boat", making.BoatMaking);
            SetCount_Procude("Bicycle_Frame", making.FrameMaking);
            SetCount_Procude("Bicycle_Wheel", making.WheelMaking);
            SetCount_Procude("Bicycle_Chain", making.ChainMaking);
            SetCount_Procude("Bicycle_Handle", making.HandleMaking);
            SetCount_Procude("Bicycle_Brake", making.BrakeMaking);
            SetCount_Procude("Bicycle_Saddle", making.SaddleMaking);
            SetCount_Procude("Bicycle", making.BikeMaking);
        }

        SewingMachineController sewing = FindObjectOfType<SewingMachineController>();
        if (sewing != null)
        {
            SetHP_Machines("SewingMachine", sewing.SewingMachine);
            sewing.SewingMachine = GetMachine("SewingMachine").hp;

            SetCount_Procude("Hat", sewing.CapMaking);
            SetCount_Procude("Glove", sewing.GroveMaking);
            SetCount_Procude("Shirt", sewing.TopMaking);
            SetCount_Procude("Pants", sewing.BottomMaking);
            SetCount_Procude("Shoes", sewing.ShoesMaking);
            SetCount_Procude("Doll", sewing.DollMaking);
        }
    }
    #endregion

    #region Ÿ ��ũ��Ʈ�� ������ ����
    public void ApplyHappyEarthGageOnLoad()
    {
        if (HappyEarth.instance != null)
        {
            if (HappyGage == -1)
            {
                HappyGage = 0;
            }
            HappyEarth.instance.setRealGage(HappyGage);
            if (Day_NUM == 1)
                HappyEarth.instance.UpdateMarkerPosition(0);
            else
            {

                float markerValue = passing_check[GetDayNum() - 1];
                //Debug.Log("��ũ��: " + markerValue);
                if (!float.IsNaN(markerValue) && !float.IsInfinity(markerValue))
                {
                    HappyEarth.instance.UpdateMarkerPosition(markerValue);
                }
                else
                {
                    Debug.LogWarning("markerValue�� ��ȿ���� �ʽ��ϴ�: " + markerValue);
                }

            }
        }
        else
        {
            Debug.LogWarning("HappyEarth.instance�� null�Դϴ�!");
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
            Debug.Log("CoinManager.instance�� null�Դϴ�.");
        }
    }
    #endregion

    #region ��������
    public void EndingChecking()
    {
        //Debug.Log("��������: ��� ��ǰ ����, �������� �̹� �˻� �� �Ѿ�� ��");
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

    #region �������

    public void SaveGame()
    {
        //Debug.Log("���ӸŴ������� ����#############");
        PlayerInvenManager.SaveSellCounts();
        SetMachineData();
        SaveManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        //Debug.Log("���ӸŴ������� �ε�.###############");
        if (itemUsages == null || machines == null || itemUsages.Count == 0 || machines.Count == 0 || Completed.Count == 0 || Completed == null)
        {
            InitializeMachines();
            InitializeItemUsages();
            InitializeCompleted();
            InitializeIngredient();
            Interiors.Clear();
        }
        SaveManager.instance.LoadGame();
        MachineDataSend();
        ReLoadInterior();
        PlayerInvenManager.LoadSellCounts();
    }


    public void ResetValues()
    {
        //Debug.Log("���ӸŴ������� ����#############");
        SetHappyGage(-1);
        SetDayNum(1);
        SetCoin(0);
        InitializeItemUsages();
        InitializeMachines();
        InitializeCompleted();
        InitializeIngredient();
        Interiors.Clear();
        PlayerInvenManager.ResetSellCounts();
    }
    #endregion
}
