using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

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
    public Button CloseBtn;

    public bool InvenMode; //인벤이 켜져 있는 상황
    private bool IsBagOn = false;

    private ShopManager shopManager;

    public bool isSelected = false;

    // 옷 인벤 변경
    public Sprite clothsprite; //옷 장착시 스프라이트

    private const string backupFileName = "manual_backup.dat";


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
                InvenClose();
                Debug.Log("인벤토리 닫힘!");
            }
        }

        CloseBtn.onClick.AddListener(() => InvenClose()); //X 버튼을 눌러 인벤 닫기

        if (Input.GetKeyDown(KeyCode.I)) //Esc키로 인벤 닫기
        {
            AddItemToHotBarOrPlayerInventory("T_Paper");
        }

        if( Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("백업 파일로 인벤토리 저장 시도");
            InventorySaveSystem.SaveInventoryWithBackup(
                InventoryController.instance.GetInventoryManager(),
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                backupFileName
            );
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("백업 파일에서 인벤토리 불러오기 시도");
            var backupData = InventorySaveSystem.LoadBackup(backupFileName);
            InventorySaveSystem.RestoreInventoryFromBackup(backupData);
            InvenClose(); // 인벤토리 닫기
        }
    }
    
    public void Debug_GetItem()
    {
        AddItemToHotBarOrPlayerInventory("T_Paper");
    }

    public void InvenClose() //열려있는 인벤창을 닫음
    {
        GameManager_ScrapLand.instance.SetSensOrigin();
        // 마우스 다시 움직일때 쓰세요

        InventoryUI.SetActive(false);
        Inventory_Cloth.SetActive(false);
        Inven_Background.SetActive(false);
        HotBar_Background.SetActive(true);

        InvenMode = false;
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
    }

}
