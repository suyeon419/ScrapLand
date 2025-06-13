using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;


public class StorageManager : MonoBehaviour
{
    public static StorageManager instance { get; private set; }

/*    private string storageName = "Storage1";
    private int storageRow = 6;
    private int storageCol = 4;
    bool StorageHighlitable = true;
    bool StorageDraggable = true;
    bool StorageSave = false;
    bool StorageActive = false;*/

    private InventoryController InventoryController;
    private PlayerInvenManager PlayerInvenManager;
    private ShopManager ShopManager;

    //hhhhhh
    public RectTransform InventoryUI;
    public RectTransform Inventory_Cloth;
    public RectTransform HotBar;
    public RectTransform Inven_Background;

    public GameObject Storage1;

    public bool isStorageOpen = false; //Storage가 열려있는지 확인하는 변수

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
{
    InventoryController = FindObjectOfType<InventoryController>();
    PlayerInvenManager = FindObjectOfType<PlayerInvenManager>();
    ShopManager = FindObjectOfType<ShopManager>();

    if (InventoryController == null)
        Debug.LogError("InventoryController를 찾을 수 없습니다.");
    if (PlayerInvenManager == null)
        Debug.LogError("PlayerInvenManager를 찾을 수 없습니다.");
    if (ShopManager == null)
        Debug.LogError("ShopManager를 찾을 수 없습니다.");
}


    // Update is called once per frame
    void Update()
    {
    }

    public void MakeStorage()
    {
/*        InventoryController.CreateInventory(storageUIPos, 
            storageName, storageRow, storageCol, StorageHighlitable,
            StorageDraggable, StorageSave, StorageActive);*/
    }

    public void OpenStorage()
    {
        isStorageOpen = true; //Storage가 열렸음을 표시

        //인벤토리 위치 이동
        PlayerInvenManager.InvenOpen();
        InventoryUI.anchoredPosition = new Vector2(-60, 110);
        Inventory_Cloth.anchoredPosition = new Vector2(395, 0);
        HotBar.anchoredPosition = new Vector2(-60, -380);
        Inven_Background.anchoredPosition = new Vector2(-160, 370);

        //StorageActive = true;
        Storage1.SetActive(true);

    }

    public void CloseStorage()
    {
        isStorageOpen = false; //Storage가 닫혔음을 표시

        //인벤토리 원래 위치 이동
        PlayerInvenManager.InvenClose();
        InventoryUI.anchoredPosition = new Vector2(-455, 100);
        Inventory_Cloth.anchoredPosition = new Vector2(0, 0);
        HotBar.anchoredPosition = new Vector2(-455, -380);
        Inven_Background.anchoredPosition = new Vector2(-555, 360);

        //StorageActive = false;
        Storage1.SetActive(false);
    }

    public void InvenMove()
    {

    }
}
