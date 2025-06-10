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
    //[SerializeField] private InventoryUIManager inventoryUIManager; // 인벤토리 매니저 연결
    
    public static ShopManager Instance { get; private set; }

    public bool ShopMode = false;
    public bool BuyMode = false;

    //UI
    public TextMeshProUGUI ShopText; //상점 아저씨 말

    public RectTransform InventoryUI;
    public RectTransform HotBar;
    public RectTransform Cloth;
    public GameObject Inven_Xbtn;
    public GameObject CloseBtn; //pressed -> shopmodeoff()
    public RectTransform Inven_Background;
    public GameObject HotBar_Background;
    public GameObject UI;

    //상점 관련
    public GameObject ShopUI;
    public GameObject BuyShopUI;

    //판매상점
    public TextMeshProUGUI machineNameText;
    public TextMeshProUGUI machineDescriptionText;
    public Image machineImage;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI FirstP;


    private PlayerInvenManager InvenManager;
    private CoinManager coinManager;
    //private InventoryUIManager inventoryUIManager;
    //선택된 아이템 리스트

    //기계 구매
    public List<MachineData> machines = new List<MachineData>();
    private MachineData selectedMachine;
    public TextMeshProUGUI machinePriceText;

    //물건 판매
    public Inventory playerInventory;
    //public ItemDatabase itemDatabase;
    //public List<int> selectedSlots; //선택된 슬롯들
    //public int playerMoney = 0;

    private void Awake()
    {
        InvenManager = FindObjectOfType<PlayerInvenManager>();
        coinManager = FindObjectOfType<CoinManager>();

        if (InvenManager == null)
        {
            Debug.LogError("PlayerInvenManager를 찾을 수 없습니다. 씬에 해당 오브젝트가 존재하는지 확인하세요.");
        }
        else
        {
            Debug.Log(" PlayerInvenManager를 성공적으로 찾았습니다.");
        }

        if (Instance != null && Instance != this) //싱글톤 패턴
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        coinManager.UpdateMoneyText(coinManager.coin);
        ShopText.text = "반갑습니다! 판매할 물건을 선택하세요.";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){ //판매 상점
            ShopModeOn();
        }
        if (Input.GetKeyDown(KeyCode.E)) //구매 상점
        {
            BuyShopModeOn();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            TrySell();
        }
    }

    public void SellingBtnClicked()
    {
        ShopMode = true;
    }

    public void ShopModeOn()
    {
        InvenManager.InvenOpen();

        //인벤토리 위치 이동
        Inven_Xbtn.SetActive(false);
        Cloth.anchoredPosition = new Vector2(-485, 0);
        Inven_Background.anchoredPosition = new Vector2(-925, 320);
        HotBar.anchoredPosition = new Vector2(-835, -220);
        InventoryUI.anchoredPosition = new Vector2(-840, 120);

        //상점 UI 활성화
        //buyModeOff();
        ShopUI.SetActive(true);
        ShopMode = true;
        BuyMode = false;
    }

    public void BuyShopModeOn()
    {
        CoinManager.Instance.UpdateMoneyText(CoinManager.Instance.coin);
        InventoryUI.gameObject.SetActive(false);
        HotBar.gameObject.SetActive(false);
        HotBar_Background.SetActive(false);


        //판매상점 UI 활성화
        //ShopModeOff();
        BuyShopUI.SetActive(true);
        moneyText.text = $"{coinManager.coin:N0}";
        ShopMode = false;
        BuyMode = true;
    }

    public void ShopModeOff()
    {
        //인벤 원래 위치로 이동 코드
        Inven_Xbtn.SetActive(true);
        Cloth.anchoredPosition = new Vector2(0, 0);
        Inven_Background.anchoredPosition = new Vector2(-435, 320);
        InventoryUI.anchoredPosition = new Vector2(-350, 120);

        InvenManager.InvenClose(); //상점을 닫고 핫바 상태로 전환
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
            Debug.Log("판매할 아이템이 선택되지 않았습니다.");
            return;
        }

        Slot slot = selectedSlot.GetComponent<Slot>();
        InventoryItem item = slot.GetItem();

        if (item != null && !item.GetIsNull())
        {
            string itemType = item.GetItemType();
            int price = item.GetItemPrice();

            coinManager.coin += price; //플레이어 코인에 추가

            //해피 지구력 적용
            /*            int HpCount = item.GetHPCount_Sell();
                        int Hp = ((int)item.GetItemHappyPoint()); //다 정수라 상관없음
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
                Debug.Log($"아이템 '{itemType}'은 해피포인트 제한 도달");
            }
            CoinManager.Instance.HappyText.text = $"{CoinManager.Instance.HappyP}";
            InventorySelectionManager.Instance.IncrementSellCount(itemType);

            coinManager.UpdateMoneyText(coinManager.coin); //UI 업데이트

            //아이템 삭제
            selectedUI.GetInventory().EraseItemInPosition(slot.GetPosition());
            selectedUI.ResetHighlight();
            InventorySelectionManager.ClearSelection(); // 선택 해제
        }
    }

    /// <summary>
    /// 기계 구매
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
            FirstP.text = "첫 구매 100 코인";
        }

        machineNameText.text = machine.machineName;
        machineDescriptionText.text = machine.description;
        machineImage.sprite = machine.image;
    }

    public void PurchaseSelectedMachine()
    {
        if (selectedMachine == null)
            return;

        if (coinManager.coin >= selectedMachine.currentPrice)
        {
            coinManager.coin -= selectedMachine.currentPrice;
            selectedMachine.isPurchased = true;

            coinManager.CoinText.text = $"Coin: {coinManager.coin:N0}";
            moneyText.text = $"{coinManager.coin:N0}";
            Debug.Log($"{selectedMachine.machineName},{selectedMachine.currentPrice} 구매 성공!");

            // 다음부터는 원래 가격으로
            selectedMachine.currentPrice = selectedMachine.originalPrice;

            // 구매 완료 후 UI 갱신 필요하면 여기에 추가
            FirstP.text = "구매 완료";
            machinePriceText.text = $"{selectedMachine.currentPrice:N0}코인";

            //인벤토리에 기계 추가
            InventoryController.instance.AddItem("HotBar", selectedMachine.relatedItem.GetItemType(), 1);
        }
        else
        {
            //Debug.Log("돈이 부족합니다!");
            FirstP.text = "돈이 부족합니다!";
        }
    }
}

//기계 클래스
[System.Serializable]
public class MachineData
{
    public string machineName;
    public string description;
    public Sprite image;
    public int originalPrice; // 원래 가격 (Inspector에 입력)

    [HideInInspector]
    public int currentPrice = 100; // 첫 구매는 100, 이후는 원래 가격

    public bool isPurchased = false;
    public ItemInitializer relatedItem;
}