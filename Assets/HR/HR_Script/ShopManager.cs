using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ShopManager : MonoBehaviour
{
    //[SerializeField] private InventoryUIManager inventoryUIManager; // �κ��丮 �Ŵ��� ����
    
    public static ShopManager Instance { get; private set; }

    public bool ShopMode = false;
    public bool BuyMode = false;

    //UI
    public TextMeshProUGUI ShopText; //���� ������ ��

    public RectTransform InventoryUI;
    public RectTransform HotBar;
    public RectTransform Cloth;
    public GameObject Inven_Xbtn;
    public RectTransform Inven_Background;
    public GameObject HotBar_Background;
    public GameObject UI;

    //���� ����
    public GameObject ShopUI;
    public GameObject BuyShopUI;

    //�ǸŻ���
    public TextMeshProUGUI machineNameText;
    public TextMeshProUGUI machineDescriptionText;
    public Image machineImage;
    public TextMeshProUGUI FirstP;


    private PlayerInvenManager InvenManager;
    private CoinManager coinManager;
    //private InventoryUIManager inventoryUIManager;
    //���õ� ������ ����Ʈ

    //��� ����
    public List<MachineData> machines = new List<MachineData>();
    private MachineData selectedMachine;
    public TextMeshProUGUI machinePriceText;
    public UnityEngine.UI.Button purchaseButton;

    private static readonly Dictionary<string, string> MachineNameMap = new Dictionary<string, string>
    {
        { "Grinder", "�м��" },
        { "SewingMachine", "���Ʋ" },
        { "Filature", "������" },
        { "Compressor", "�����" },
        { "BlastFurnace", "�뱤��" }
    };


    //���� �Ǹ�
    public Inventory playerInventory;
    //public ItemDatabase itemDatabase;
    //public List<int> selectedSlots; //���õ� ���Ե�
    //public int playerMoney = 0;

    private void Awake()
    {
        InvenManager = FindObjectOfType<PlayerInvenManager>();
        coinManager = FindObjectOfType<CoinManager>();

        if (InvenManager == null)
        {
            Debug.LogError("PlayerInvenManager�� ã�� �� �����ϴ�. ���� �ش� ������Ʈ�� �����ϴ��� Ȯ���ϼ���.");
        }
        else
        {
            Debug.Log(" PlayerInvenManager�� ���������� ã�ҽ��ϴ�.");
        }

        if (Instance != null && Instance != this) //�̱��� ����
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        coinManager.UpdateMoneyText(coinManager.coin);
        ShopText.text = "�ݰ����ϴ�! �Ǹ��� ������ �����ϼ���.";
    }

    private void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Q)){ //�Ǹ� ����
            ShopModeOn();
        }
        if (Input.GetKeyDown(KeyCode.E)) //���� ����
        {
            BuyShopModeOn();
        }*/

    }

    public void SellingBtnClicked()
    {
        ShopMode = true;
    }

    public void ShopModeOn()
    {
        //Camera.enabled = false;
        InvenManager.InvenOpen();

        //�κ��丮 ��ġ �̵�
        Inven_Xbtn.SetActive(false);
        Cloth.anchoredPosition = new Vector2(-380, 0);
        Inven_Background.anchoredPosition = new Vector2(-935, 360);
        HotBar.anchoredPosition = new Vector2(-835, -380);
        InventoryUI.anchoredPosition = new Vector2(-835, 100);

        //���� UI Ȱ��ȭ
        //buyModeOff();
        ShopUI.SetActive(true);
        ShopMode = true;
        BuyMode = false;
    }

    public void BuyShopModeOn()
    {
        UpdateAllMachinePrices(); // ��� ����� ���� ������Ʈ

        CoinManager.Instance.UpdateMoneyText(CoinManager.Instance.coin);
        InventoryUI.gameObject.SetActive(false);
        HotBar.gameObject.SetActive(false);
        HotBar_Background.SetActive(false);

        //�ǸŻ��� UI Ȱ��ȭ
        //ShopModeOff();
        BuyShopUI.SetActive(true);
        ShopMode = false;
        BuyMode = true;
    }

    public void ShopModeOff()
    {
        //�κ� ���� ��ġ�� �̵� �ڵ�
        InventoryUI.anchoredPosition = new Vector2(-455, 100);
        Cloth.anchoredPosition = new Vector2(0, 0);
        HotBar.anchoredPosition = new Vector2(-455, -380);
        Inven_Background.anchoredPosition = new Vector2(-555, 360);
        Inven_Xbtn.SetActive(true);

        InvenManager.InvenClose(); //������ �ݰ� �ֹ� ���·� ��ȯ
        ShopUI.SetActive(false);
        ShopMode = false;
    }

    public void buyModeOff()
    {
        InvenManager.InvenClose();
        HotBar.gameObject.SetActive(true);
        BuyShopUI.SetActive(false);
        BuyMode = false;
    }

    public void TrySell()
    {
        var selectedSlot = InventorySelectionManager.SelectedSlot;
        var selectedUI = InventorySelectionManager.SelectedInventoryUI;

        if (selectedSlot == null || selectedUI == null)
        {
            Debug.Log("�Ǹ��� �������� ���õ��� �ʾҽ��ϴ�.");
            return;
        }

        Slot slot = selectedSlot.GetComponent<Slot>();
        InventoryItem item = slot.GetItem();

        if (item != null && !item.GetIsNull())
        {
            string itemType = item.GetItemType();
            int price = item.GetItemPrice();

            coinManager.coin += price; //�÷��̾� ���ο� �߰�

            //���� ������ ����;
            int sellCount = PlayerInvenManager.GetSellCount(itemType);
            if (sellCount < 2)
            {
                CoinManager.Instance.HappyP += (int)item.GetItemHappyPoint();
                HappyEarth.instance.PlusGageBySale((int)item.GetItemHappyPoint());
            }
            else
            {
                Debug.Log($"������ '{itemType}'�� ��������Ʈ ���� ����");
            }
            //CoinManager.Instance.HappyText.text = $"{CoinManager.Instance.HappyP}";
            //������ ��������Ʈ ���

            //�Ǹ� Ƚ�� ����
            PlayerInvenManager.IncrementSellCount(itemType);

            coinManager.UpdateMoneyText(coinManager.coin); //UI ������Ʈ

            //������ ����
            selectedUI.GetInventory().EraseItemInPosition(slot.GetPosition());
            //selectedUI.ResetHighlight();
            //InventorySelectionManager.ClearSelection(); // ���� ����

            //�տ� ��� ���� ����
            InventorySelectionManager.SetSelection(selectedSlot, selectedUI);

        }
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="index"></param>

    /*    public void SelectMachine(int index)
        {
            if (index < 0 || index >= machines.Count)
                return;

            selectedMachine = machines[index];
            UpdateMachineUI(selectedMachine);
        }*/

    /*    public void UpdateMachineUI(MachineData machine)
        {
            if (selectedMachine.isOnMap)
            {
                FirstP.text = "�̹� ��ġ�� ����Դϴ�.";
                Debug.Log("�̹� ��ġ�� ����Դϴ�.");
                purchaseButton.interactable = false; // ���� ��ư ��Ȱ��ȭ
            }
            if (!selectedMachine.isPurchased)
            {
                FirstP.text = "ù ���� 100 ����";
            }

            machineNameText.text = machine.machineName;
            machineDescriptionText.text = machine.description;
            machineImage.sprite = machine.image;
        }*/

    public void SelectMachine(int index)
    {
        if (index < 0 || index >= machines.Count)
            return;

        selectedMachine = machines[index];
        UpdateMachineUI(selectedMachine);
    }

    public void UpdateMachineUI(MachineData machine)
    {
        if (machine.isOnMap)
        {
            FirstP.text = "�̹� ��ġ�� ����Դϴ�.";
            Debug.Log("�̹� ��ġ�� ����Դϴ�.");
            purchaseButton.interactable = false; // ���� ��ư ��Ȱ��ȭ
        }
        else if (!machine.isPurchased)
        {
            FirstP.text = "ù ���� 100 ����";
            purchaseButton.interactable = true;
        }
        else
        {
            purchaseButton.interactable = true;
        }

        machineNameText.text = machine.machineName;
        machineDescriptionText.text = machine.description;
        machineImage.sprite = machine.image;
    }

    public void UpdateAllMachinePrices()
    {
        foreach (var machine in machines)
        {
            if (!machine.isPurchased)
            {
                machine.priceText.text = "100 ����";
            }
            else
            {
                machine.priceText.text = $"{machine.originalPrice:N0} ����";
            }
        }
    }


    public void PurchaseSelectedMachine()
    {
        if (selectedMachine == null)
            return;

        if (selectedMachine.isOnMap)
        {
            FirstP.text = "�̹� ��ġ�� ����Դϴ�.";
            Debug.Log("�̹� ��ġ�� ����Դϴ�.");
            return;
        }

        if (coinManager.coin >= selectedMachine.currentPrice)
        {
            coinManager.coin -= selectedMachine.currentPrice;
            selectedMachine.isPurchased = true;

            coinManager.CoinText.text = $"Coin: {coinManager.coin:N0}";
            Debug.Log($"{selectedMachine.machineName},{selectedMachine.currentPrice} ���� ����!");

            // �������ʹ� ���� ��������
            selectedMachine.currentPrice = selectedMachine.originalPrice;

            // ���� �Ϸ� �� UI ���� �ʿ��ϸ� ���⿡ �߰�
            FirstP.text = "���� �Ϸ�";
            machinePriceText.text = $"{selectedMachine.currentPrice:N0}����";

            //�κ��丮�� ��� �߰�
            InventoryController.instance.AddItem("HotBar", selectedMachine.relatedItem.GetItemType(), 1);

            //selectedMachine.isOnMap = true; // �ʿ� ��ġ ���·� ����
        }
        else
        {
            //Debug.Log("���� �����մϴ�!");
            FirstP.text = "���� �����մϴ�!";
        }

        UpdateAllMachinePrices(); // ��� ����� ���� ������Ʈ
    }

    public void GetOnMachine(string machineName, bool value)
    {
        // ���� �̸��̸� �ѱ۷� ��ȯ
        if (MachineNameMap.TryGetValue(machineName, out string koreanName))
        {
            machineName = koreanName;
        }

        foreach (var machine in machines)
        {
            if (machine.machineName.Trim().Equals(machineName.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                machine.isOnMap = value;
                Debug.Log($"{machineName}�� isOnMap�� {value}�� �����Ǿ����ϴ�.");

                // ���õ� ��谡 �� ����� UI ����
                if (selectedMachine == machine)
                {
                    UpdateMachineUI(selectedMachine);
                }
                return;
            }
        }
        Debug.LogWarning($"{machineName} �̸��� ��踦 ã�� �� �����ϴ�.");
    }

}

//��� Ŭ����
[System.Serializable]
public class MachineData
{
    public string machineName;
    public string description;
    public Sprite image;
    public int originalPrice; // ���� ���� (Inspector�� �Է�)

    [HideInInspector]
    public int currentPrice = 100; // ù ���Ŵ� 100, ���Ĵ� ���� ����

    public bool _isPurchased = false;
    public bool _isOnMap = false; // �ʿ� ��ġ ����

    public ItemInitializer relatedItem;

    public TextMeshProUGUI priceText; // UI�� ǥ���� ���� �ؽ�Ʈ

    // isPurchased ������Ƽ (����/����)
    public bool isPurchased
    {
        get { return _isPurchased; }
        set { _isPurchased = value; }
    }

    public bool isOnMap
    {
        get { return _isOnMap; }
        set { _isOnMap = value; }
    }
}