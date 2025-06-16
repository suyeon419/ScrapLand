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
    //싱글톤 패턴

    //private string SavePath => Application.persistentDataPath + "/Player_save.json";

    [Tooltip("토글할 인벤토리 오브젝트")]
    public GameObject InventoryUI;
    public GameObject Inventory_Cloth;
    public GameObject Inven_Background;
    public GameObject HotBar_Background;
    public RectTransform HotBar_Bar;
    //public Button CloseBtn;

    public bool InvenMode; //인벤이 켜져 있는 상황
    public bool IsBagOn = false; //내부 인벤 확장

    private ShopManager shopManager;

    public bool isSelected = false;

    // 옷 인벤 변경
    public Sprite clothsprite; //옷 장착시 스프라이트
    public InventoryUIManager bagInventoryUIManager;

    private const string backupFileName = "manual_backup.dat";

    //아이템 해피포인트 지급
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
        InvenMode = false; //==핫바모드. 상점코드에서 사용

        if (!IsBagOn)
        {
            InventoryUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //Tab키로 인벤 열고 닫기
        {
            if (!InvenMode) //==InvenMode false
            {
                InvenOpen();
                Debug.Log("인벤토리 열림!");
            }
            else
            {
                if (StorageManager.instance.isStorageOpen)
                {
                    StorageManager.instance.CloseStorage(); // 인벤토리 닫기 전에 스토리지 닫기
                }
                else
                {
                    InvenClose();
                    Debug.Log("인벤토리 닫힘!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) //Esc키로 인벤 닫기
        {
            AddItemToHotBarOrPlayerInventory("Old Chest");
        }

        /*        if (Input.GetKeyDown(KeyCode.I))
                {
                    AddItemToHotBarOrPlayerInventory("Old Chest");
                }

                if( Input.GetKeyDown(KeyCode.M))
                {
                    Debug.Log("백업 파일로 인벤토리 저장 시도");
                    InventorySaveSystem.SaveInventoryWithBackup(
                        InventoryController.instance.GetInventoryManager(),
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                        backupFileName
                    );
                }*/

        /*        if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log("백업 파일에서 인벤토리 불러오기 시도");
                    var backupData = InventorySaveSystem.LoadBackup(backupFileName);
                    InventorySaveSystem.RestoreInventoryFromBackup(backupData);
                    InvenClose(); // 인벤토리 닫기
                }*/
    }

    public void Debug_GetItem()
    {
        AddItemToHotBarOrPlayerInventory("T_Paper");
    }

    public void InvenClose() //열려있는 인벤창을 닫음
    {
        if( StorageManager.instance.isStorageOpen)
        {
            StorageManager.instance.CloseStorage(); // 인벤토리 닫기 전에 스토리지 닫기
        }

        InventoryUI.SetActive(false);
        Inventory_Cloth.SetActive(false);
        Inven_Background.SetActive(false);
        HotBar_Background.SetActive(true);

        InvenMode = false;
        GameManager_ScrapLand.instance.SetSensOrigin();
        // 마우스 다시 움직일때 쓰세요
    }

    public void InvenOpen() //인벤창을 열고 핫바 위치 이동
    {
        GlobalCanvasManager.instance.StopCamMoving();
        // 마우스 멈출때 쓰시고

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
            targetImage.sprite = clothsprite; //옷 장착시 스프라이트 변경
        }
        else
        {
            Debug.LogWarning("Target image or cloth sprite is not set.");
        }
    }

    //HH: AddItemToHotBarOrPlayerInventory 메서드 추가
    public void AddItemToHotBarOrPlayerInventory(string itemType, int amount = 1)
    {
        string hotBarName = "HotBar";
        string playerInventoryName = "PlayerInventory";

        // 1. 핫바가 가득 찼는지 확인
        bool isHotBarFull = InventoryController.instance.InventoryFull(hotBarName, itemType);

        if (!isHotBarFull)
        {
            // 핫바에 공간이 있으면 핫바에 추가
            InventoryController.instance.AddItem(hotBarName, itemType, amount);
        }
        else
        {
            // 핫바가 가득 찼고, PlayerInventory가 활성화된 경우
            if (IsBagOn)
            {
                InventoryController.instance.AddItem(playerInventoryName, itemType, amount);
            }
            else
            {
                // PlayerInventory가 비활성화 상태라면 원하는 동작(예: 무시, 대기 등) 추가
                Debug.Log("PlayerInventory가 비활성화 상태입니다.");
            }
        }

        InventorySelectionManager.Instance.OnSlotClicked();

    }

    // 아이템별 해피포인트 저장 및 불러오기
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
