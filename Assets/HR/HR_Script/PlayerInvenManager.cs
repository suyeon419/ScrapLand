using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Text;
using Controller;

[System.Serializable]
public class PlayerInvenManager : MonoBehaviour
{
    public static PlayerInvenManager instance;
    //�̱��� ����

    //private string SavePath => Application.persistentDataPath + "/Player_save.json";

    [Tooltip("����� �κ��丮 ������Ʈ")]
    public GameObject InventoryUI;
    public GameObject Inventory_Cloth;
    public GameObject Inven_Background;
    public GameObject HotBar_Background;
    public RectTransform HotBar_Bar;
    //public Button CloseBtn;

    public bool InvenMode; //�κ��� ���� �ִ� ��Ȳ
    public bool IsBagOn = false; //���� �κ� Ȯ��

    private ShopManager shopManager;

    public bool isSelected = false;

    // �� �κ� ����
    public Sprite clothsprite; //�� ������ ��������Ʈ
    public InventoryUIManager bagInventoryUIManager;

    private const string backupFileName = "manual_backup.dat";

    //������ ��������Ʈ ����
    private static Dictionary<string, int> itemSellCounts = new Dictionary<string, int>();


    [System.Serializable]
    public class Serialization<TKey, TValue>
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();
        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public Serialization(Dictionary<TKey, TValue> dict)
        {
            foreach (var pair in dict)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict[keys[i]] = values[i];
            }
            return dict;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();


        InvenClose();
        InvenMode = false; //==�ֹٸ��. �����ڵ忡�� ���

        if (!IsBagOn)
        {
            InventoryUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //TabŰ�� �κ� ���� �ݱ�
        {
            if (!InvenMode) //==InvenMode false
            {
                InvenOpen();
                Debug.Log("�κ��丮 ����!");
            }
            else
            {
                if (StorageManager.instance.isStorageOpen)
                {
                    StorageManager.instance.CloseStorage(); // �κ��丮 �ݱ� ���� ���丮�� �ݱ�
                }
                else
                {
                    InvenClose();
                    Debug.Log("�κ��丮 ����!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) //EscŰ�� �κ� �ݱ�
        {
            AddItemToHotBarOrPlayerInventory("Old Chest");
        }

        /*        if (Input.GetKeyDown(KeyCode.I))
                {
                    AddItemToHotBarOrPlayerInventory("Old Chest");
                }

                if( Input.GetKeyDown(KeyCode.M))
                {
                    Debug.Log("��� ���Ϸ� �κ��丮 ���� �õ�");
                    InventorySaveSystem.SaveInventoryWithBackup(
                        InventoryController.instance.GetInventoryManager(),
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                        backupFileName
                    );
                }*/

        /*        if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log("��� ���Ͽ��� �κ��丮 �ҷ����� �õ�");
                    var backupData = InventorySaveSystem.LoadBackup(backupFileName);
                    InventorySaveSystem.RestoreInventoryFromBackup(backupData);
                    InvenClose(); // �κ��丮 �ݱ�
                }*/
    }

    public void Debug_GetItem()
    {
        AddItemToHotBarOrPlayerInventory("T_Paper");
    }

    public void InvenClose() //�����ִ� �κ�â�� ����
    {
        if( StorageManager.instance.isStorageOpen)
        {
            StorageManager.instance.CloseStorage(); // �κ��丮 �ݱ� ���� ���丮�� �ݱ�
        }

        InventoryUI.SetActive(false);
        Inventory_Cloth.SetActive(false);
        Inven_Background.SetActive(false);
        HotBar_Background.SetActive(true);

        InvenMode = false;
        GameManager_ScrapLand.instance.SetSensOrigin();
        // ���콺 �ٽ� �����϶� ������
    }

    public void InvenOpen() //�κ�â�� ���� �ֹ� ��ġ �̵�
    {
        GlobalCanvasManager.instance.StopCamMoving();
        // ���콺 ���⶧ ���ð�

        if (IsBagOn) {
            InventoryUI.SetActive(true);
        }
        
        Inventory_Cloth.SetActive(true);
        Inven_Background.SetActive(true);
        HotBar_Background.SetActive(false);

        InvenMode = true;
    }
    
    public void BagOn()
    {
        InventoryUI.SetActive(true);
        IsBagOn = true;

        bagInventoryUIManager.SetDraggable(false);
        bagInventoryUIManager.SetHighlightable(false);

    }

    public void BagOff()
    {
        InventoryUI.SetActive(false);
        IsBagOn = false;
    }

    public void ShopeModeOn()
    {
        if (shopManager != null && shopManager.ShopMode)
        {
            ToggleSelection();
        }
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            //shopManager.AddSelectedItem(item);
        }
        else
        {
            //shopManager.RemoveSelectedItem(item);
        }
    }

    public void ClothInven()
    {
        Image targetImage = GetComponent<Image>();

        if(targetImage != null && clothsprite != null)
        {
            targetImage.sprite = clothsprite; //�� ������ ��������Ʈ ����
        }
        else
        {
            Debug.LogWarning("Target image or cloth sprite is not set.");
        }
    }

    //HH: AddItemToHotBarOrPlayerInventory �޼��� �߰�
    public void AddItemToHotBarOrPlayerInventory(string itemType, int amount = 1)
    {
        string hotBarName = "HotBar";
        string playerInventoryName = "PlayerInventory";

        // 1. �ֹٰ� ���� á���� Ȯ��
        bool isHotBarFull = InventoryController.instance.InventoryFull(hotBarName, itemType);

        if (!isHotBarFull)
        {
            // �ֹٿ� ������ ������ �ֹٿ� �߰�
            InventoryController.instance.AddItem(hotBarName, itemType, amount);
        }
        else
        {
            // �ֹٰ� ���� á��, PlayerInventory�� Ȱ��ȭ�� ���
            if (IsBagOn)
            {
                InventoryController.instance.AddItem(playerInventoryName, itemType, amount);
            }
            else
            {
                // PlayerInventory�� ��Ȱ��ȭ ���¶�� ���ϴ� ����(��: ����, ��� ��) �߰�
                Debug.Log("PlayerInventory�� ��Ȱ��ȭ �����Դϴ�.");
            }
        }

        InventorySelectionManager.Instance.OnSlotClicked();

    }

    // �����ۺ� ��������Ʈ ���� �� �ҷ�����
    public static void SaveSellCounts()
    {
        string path = Application.persistentDataPath + "/sellCounts.json";
        string json = JsonUtility.ToJson(new Serialization<string, int>(itemSellCounts));
        File.WriteAllText(path, json, Encoding.UTF8);
    }

    public static void LoadSellCounts()
    {
        string path = Application.persistentDataPath + "/sellCounts.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path, Encoding.UTF8);
            var data = JsonUtility.FromJson<Serialization<string, int>>(json);
            itemSellCounts = data.ToDictionary();
        }
    }

    public static void ResetSellCounts()
    {
        string path = Application.persistentDataPath + "/sellCounts.json";
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static int GetSellCount(string itemType)
    {
        if (itemSellCounts.TryGetValue(itemType, out int count))
            return count;
        return 0;
    }

    public static void IncrementSellCount(string itemType)
    {
        if (itemSellCounts.ContainsKey(itemType))
            itemSellCounts[itemType]++;
        else
            itemSellCounts[itemType] = 1;
    }
}
