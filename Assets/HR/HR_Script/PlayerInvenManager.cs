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
    //�̱��� ����

    //private string SavePath => Application.persistentDataPath + "/Player_save.json";

    [Tooltip("����� �κ��丮 ������Ʈ")]
    public GameObject InventoryUI;
    public GameObject Inventory_Cloth;
    public GameObject Inven_Background;
    public GameObject HotBar_Background;
    public RectTransform HotBar_Bar;
    public Button CloseBtn;

    public bool InvenMode; //�κ��� ���� �ִ� ��Ȳ
    private bool IsBagOn = false;

    private ShopManager shopManager;

    public bool isSelected = false;

    // �� �κ� ����
    public Sprite clothsprite; //�� ������ ��������Ʈ

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
                InvenClose();
                Debug.Log("�κ��丮 ����!");
            }
        }

        CloseBtn.onClick.AddListener(() => InvenClose()); //X ��ư�� ���� �κ� �ݱ�

        if (Input.GetKeyDown(KeyCode.I)) //EscŰ�� �κ� �ݱ�
        {
            AddItemToHotBarOrPlayerInventory("T_Paper");
        }

        if( Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("��� ���Ϸ� �κ��丮 ���� �õ�");
            InventorySaveSystem.SaveInventoryWithBackup(
                InventoryController.instance.GetInventoryManager(),
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                backupFileName
            );
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("��� ���Ͽ��� �κ��丮 �ҷ����� �õ�");
            var backupData = InventorySaveSystem.LoadBackup(backupFileName);
            InventorySaveSystem.RestoreInventoryFromBackup(backupData);
            InvenClose(); // �κ��丮 �ݱ�
        }
    }
    
    public void Debug_GetItem()
    {
        AddItemToHotBarOrPlayerInventory("T_Paper");
    }

    public void InvenClose() //�����ִ� �κ�â�� ����
    {
        GameManager_ScrapLand.instance.SetSensOrigin();
        // ���콺 �ٽ� �����϶� ������

        InventoryUI.SetActive(false);
        Inventory_Cloth.SetActive(false);
        Inven_Background.SetActive(false);
        HotBar_Background.SetActive(true);

        InvenMode = false;
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
    }

}
