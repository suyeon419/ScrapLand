using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;


public class StorageManager : MonoBehaviour
{
    public static StorageManager instance { get; private set; }

    public Transform storageUIPos;
    private string storageName = "Storage1";
    private int storageRow = 6;
    private int storageCol = 4;
    bool StorageHighlitable = true;
    bool StorageDraggable = true;
    bool StorageSave = false;
    bool StorageActive = false;

    private InventoryController InventoryController;
    private PlayerInvenManager PlayerInvenManager;
    private ShopManager ShopManager;

    //hhhhhh
    public RectTransform InventoryUI;
    public RectTransform Inventory_Cloth;
    public RectTransform HotBar;
    public RectTransform Inven_Background;

    public GameObject Storage1;


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
        InventoryController.CreateInventory(storageUIPos, 
            storageName, storageRow, storageCol, StorageHighlitable,
            StorageDraggable, StorageSave, StorageActive);
    }

    public void OpenStorage()
    {
        //인벤토리 위치 이동
        PlayerInvenManager.InvenOpen();

        InventoryUI.anchoredPosition = new Vector2(-50, 120);
        Inventory_Cloth.anchoredPosition = new Vector2(300, 0);
        HotBar.anchoredPosition = new Vector2(-55, -220);
        Inven_Background.anchoredPosition = new Vector2(-135, 320);

        //StorageActive = true;
        Storage1.SetActive(true);

    }

    public void CloseStorage()
    {
        //인벤토리 위치 이동
        PlayerInvenManager.InvenClose();
        InventoryUI.anchoredPosition = new Vector2(-350, 120);
        Inventory_Cloth.anchoredPosition = new Vector2(0, 0);
        Inven_Background.anchoredPosition = new Vector2(-435, 320);

        //StorageActive = false;
        Storage1.SetActive(false);
    }

    public void InvenMove()
    {

    }
}
