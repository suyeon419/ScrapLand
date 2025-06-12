using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public GameObject CloseBtn; //pressed -> shopmodeoff()
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
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI FirstP;


    private PlayerInvenManager InvenManager;
    private CoinManager coinManager;
    //private InventoryUIManager inventoryUIManager;
    //���õ� ������ ����Ʈ

    //��� ����
    public List<MachineData> machines = new List<MachineData>();
    private MachineData selectedMachine;
    public TextMeshProUGUI machinePriceText;

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
        Cloth.anchoredPosition = new Vector2(-400, 0);
        Inven_Background.anchoredPosition = new Vector2(-955, 360);
        HotBar.anchoredPosition = new Vector2(-855, -380);
        InventoryUI.anchoredPosition = new Vector2(-855, 100);

/*        PlayerInvenManager.InvenClose();
        InventoryUI.anchoredPosition = new Vector2(-455, 100);
        Inventory_Cloth.anchoredPosition = new Vector2(0, 0);
        HotBar.anchoredPosition = new Vector2(-455, -380);
        Inven_Background.anchoredPosition = new Vector2(-555, 360);*/

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
        moneyText.text = $"{coinManager.coin:N0}";
        ShopMode = false;
        BuyMode = true;
    }

    public void ShopModeOff()
    {
        //�κ� ���� ��ġ�� �̵� �ڵ�
        Inven_Xbtn.SetActive(true);
        Cloth.anchoredPosition = new Vector2(0, 0);
        Inven_Background.anchoredPosition = new Vector2(-555, 360);
        InventoryUI.anchoredPosition = new Vector2(-455, -100);
        InvenManager.HotBar_Bar.anchoredPosition = new Vector2(-455, -380);

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

            //���� ������ ����
            /*            int HpCount = item.GetHPCount_Sell();
                        int Hp = ((int)item.GetItemHappyPoint()); //�� ������ �������
                        if (HpCount < 3)
                        {
                            CoinManager.Instance.HappyP += Hp;
                        }
                        HpCount++;
                        item.SetHPCount_Sell(HpCount);*/
            int sellCount = InventorySelectionManager.Instance.GetSellCount(itemType);
            if (sellCount < 2)
            {
                CoinManager.Instance.HappyP += (int)item.GetItemHappyPoint();
            }
            else
            {
                Debug.Log($"������ '{itemType}'�� ��������Ʈ ���� ����");
            }
            CoinManager.Instance.HappyText.text = $"{CoinManager.Instance.HappyP}";
            InventorySelectionManager.Instance.IncrementSellCount(itemType);

            coinManager.UpdateMoneyText(coinManager.coin); //UI ������Ʈ

            //������ ����
            selectedUI.GetInventory().EraseItemInPosition(slot.GetPosition());
            selectedUI.ResetHighlight();
            InventorySelectionManager.ClearSelection(); // ���� ����
        }
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="index"></param>

    public void SelectMachine(int index)
    {
        if (index < 0 || index >= machines.Count)
            return;

        selectedMachine = machines[index];
        UpdateMachineUI(selectedMachine);
    }

    public void UpdateMachineUI(MachineData machine)
    {
        if (!selectedMachine.isPurchased)
        {
            FirstP.text = "ù ���� 100 ����";
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
            return;
        }

        if (coinManager.coin >= selectedMachine.currentPrice)
        {
            coinManager.coin -= selectedMachine.currentPrice;
            selectedMachine.isPurchased = true;

            coinManager.CoinText.text = $"Coin: {coinManager.coin:N0}";
            moneyText.text = $"{coinManager.coin:N0}";
            Debug.Log($"{selectedMachine.machineName},{selectedMachine.currentPrice} ���� ����!");

            // �������ʹ� ���� ��������
            selectedMachine.currentPrice = selectedMachine.originalPrice;

            // ���� �Ϸ� �� UI ���� �ʿ��ϸ� ���⿡ �߰�
            FirstP.text = "���� �Ϸ�";
            machinePriceText.text = $"{selectedMachine.currentPrice:N0}����";

            //�κ��丮�� ��� �߰�
            InventoryController.instance.AddItem("HotBar", selectedMachine.relatedItem.GetItemType(), 1);
        }
        else
        {
            //Debug.Log("���� �����մϴ�!");
            FirstP.text = "���� �����մϴ�!";
        }

        UpdateAllMachinePrices(); // ��� ����� ���� ������Ʈ
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