using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    public TextMeshProUGUI FirstP;


    private PlayerInvenManager InvenManager;
    private CoinManager coinManager;
    //private InventoryUIManager inventoryUIManager;
    //선택된 아이템 리스트

    //기계 구매
    public List<MachineData> machines = new List<MachineData>();
    private MachineData selectedMachine;
    public TextMeshProUGUI machinePriceText;
    public UnityEngine.UI.Button purchaseButton;

    private static readonly Dictionary<string, string> MachineNameMap = new Dictionary<string, string>
    {
        { "Grinder", "분쇄기" },
        { "SewingMachine", "재봉틀" },
        { "Filature", "방적기" },
        { "Compressor", "압축기" },
        { "BlastFurnace", "용광로" }
    };


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
        /*        if (Input.GetKeyDown(KeyCode.Q)){ //판매 상점
                    ShopModeOn();
                }
                if (Input.GetKeyDown(KeyCode.E)) //구매 상점
                {
                    BuyShopModeOn();
                }*/
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetOnMachine("Grinder", false);
        }
    }

    public void SellingBtnClicked()
    {
        ShopMode = true;
    }

    public void ShopModeOn()
    {
        //Camera.enabled = false;
        InvenManager.InvenOpen();

        //인벤토리 위치 이동
        Inven_Xbtn.SetActive(false);
        Cloth.anchoredPosition = new Vector2(-380, 0);
        Inven_Background.anchoredPosition = new Vector2(-935, 360);
        HotBar.anchoredPosition = new Vector2(-835, -380);
        InventoryUI.anchoredPosition = new Vector2(-835, 100);

        //상점 UI 활성화
        //buyModeOff();
        ShopUI.SetActive(true);
        ShopMode = true;
        BuyMode = false;
    }

    public void BuyShopModeOn()
    {
        UpdateAllMachinePrices(); // 모든 기계의 가격 업데이트

        CoinManager.Instance.UpdateMoneyText(CoinManager.Instance.coin);
        InventoryUI.gameObject.SetActive(false);
        HotBar.gameObject.SetActive(false);
        HotBar_Background.SetActive(false);

        bool CheckInvenFull = InventorySelectionManager.Instance.CheckInvenFull();
        Debug.Log($"인벤토리 가득 찼는지 확인: {CheckInvenFull}");

        if (InventorySelectionManager.Instance.CheckInvenFull())
            purchaseButton.interactable = false; // 인벤토리가 가득 차면 비활성화
        else
            purchaseButton.interactable = true;  // 아니면 활성화

        //판매상점 UI 활성화
        //ShopModeOff();
        BuyShopUI.SetActive(true);
        ShopMode = false;
        BuyMode = true;
    }

    public void ShopModeOff()
    {
        //인벤 원래 위치로 이동 코드
        InventoryUI.anchoredPosition = new Vector2(-455, 100);
        Cloth.anchoredPosition = new Vector2(0, 0);
        HotBar.anchoredPosition = new Vector2(-455, -380);
        Inven_Background.anchoredPosition = new Vector2(-555, 360);
        Inven_Xbtn.SetActive(true);

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

            //해피 지구력 적용;
            int sellCount = PlayerInvenManager.GetSellCount(itemType);
            if (sellCount < 2)
            {
                CoinManager.Instance.HappyP += (int)item.GetItemHappyPoint();
                HappyEarth.instance.PlusGageBySale((int)item.GetItemHappyPoint());
            }
            else
            {
                Debug.Log($"아이템 '{itemType}'은 해피포인트 제한 도달");
            }

            //판매 횟수 증가
            PlayerInvenManager.IncrementSellCount(itemType);

            coinManager.UpdateMoneyText(coinManager.coin); //UI 업데이트

            //아이템 삭제
            selectedUI.GetInventory().EraseItemInPosition(slot.GetPosition());
            //selectedUI.ResetHighlight();
            //InventorySelectionManager.ClearSelection(); // 선택 해제

            //손에 드는 상태 갱신
            InventorySelectionManager.SetSelection(selectedSlot, selectedUI);
            // 아이템 판매 후 해당 슬롯이 비었으면 PlacementManager에 "empty" 전달
            InventoryItem afterItem = slot.GetItem();
            if (afterItem == null || afterItem.GetIsNull())
            {
                if (PlacementManager.Instance != null)
                    PlacementManager.Instance.SetHeldItem("empty");
            }

        }
    }

    /// <summary>
    /// 기계 구매
    /// </summary>
    /// <param name="index"></param>

    public void SelectMachine(int index)
    {
        bool CheckInvenFull = InventorySelectionManager.Instance.CheckInvenFull();
        Debug.Log($"인벤토리 가득 찼는지 확인: {CheckInvenFull}");

        if (InventorySelectionManager.Instance.CheckInvenFull())
        {
            purchaseButton.interactable = false; // 인벤토리가 가득 차면 비활성화
            FirstP.text = "인벤토리가 가득 찼습니다!"; // 메시지 업데이트
        }
        else
        {
            purchaseButton.interactable = true;  // 아니면 활성화
            FirstP.text = "구매할 기계를 선택하세요";
        }
        if (index < 0 || index >= machines.Count)
            return;

        selectedMachine = machines[index];
        UpdateMachineUI(selectedMachine);
    }

    public void UpdateMachineUI(MachineData machine)
    {
        if (machine.isOnMap)
        {
            FirstP.text = "이미 설치된 기계입니다.";
            Debug.Log("이미 설치된 기계입니다.");
            purchaseButton.interactable = false; // 구매 버튼 비활성화
        }
        
        if (!machine.isPurchased)
        {
            FirstP.text = "첫 구매 100 코인";
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
                machine.priceText.text = "100 코인";
            }
            else
            {
                machine.priceText.text = $"{machine.originalPrice:N0} 코인";
            }
        }
    }


    public void PurchaseSelectedMachine()
    {

        if (selectedMachine == null)
            return;

        if (selectedMachine.isOnMap)
        {
            FirstP.text = "이미 설치된 기계입니다.";
            Debug.Log("이미 설치된 기계입니다.");
            return;
        }

        if (InventorySelectionManager.Instance.CheckInvenFull())
        {
            purchaseButton.interactable = false; // 인벤토리가 가득 차면 비활성화
            FirstP.text = "인벤토리가 \n가득 찼습니다!"; // 메시지 업데이트
            return;
        }

        if (coinManager.coin >= selectedMachine.currentPrice)
        {
            if(!selectedMachine.isPurchased)
            {
                coinManager.coin -= 100; // 첫 구매는 100 코인
                selectedMachine.isPurchased = true;
            }
            else
            {
                coinManager.coin -= selectedMachine.originalPrice; // 코인 차감
            }
            BlockController.Instance.ForceUIUpdate();

            coinManager.CoinText.text = $"Coin: {coinManager.coin:N0}";
            Debug.Log($"{selectedMachine.machineName},{selectedMachine.currentPrice} 구매 성공!");

            // 다음부터는 원래 가격으로
            //selectedMachine.currentPrice = selectedMachine.originalPrice;
            Debug.Log(selectedMachine.isPurchased);

            // 구매 완료 후 UI 갱신 필요하면 여기에 추가
            FirstP.text = "구매 완료";
            machinePriceText.text = $"{selectedMachine.currentPrice:N0}코인";

            //인벤토리에 기계 추가
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory(selectedMachine.relatedItem.GetItemType());
            
            selectedMachine.isOnMap = true; // false 받기 전까지는 구매 불가
        }
        else
        {
            //Debug.Log("돈이 부족합니다!");
            FirstP.text = "돈이 부족합니다!";
        }

        UpdateAllMachinePrices(); // 모든 기계의 가격 업데이트
    }

    public void GetOnMachine(string machineName, bool value)
    {
        // 영어 이름이면 한글로 변환
        if (MachineNameMap.TryGetValue(machineName, out string koreanName))
        {
            machineName = koreanName;
        }

        foreach (var machine in machines)
        {
            if (machine.machineName.Trim().Equals(machineName.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                machine.isOnMap = value;
                Debug.Log($"{machineName}의 isOnMap이 {value}로 설정되었습니다.");

                // 선택된 기계가 이 기계라면 UI 갱신
                if (selectedMachine == machine)
                {
                    UpdateMachineUI(selectedMachine);
                }
                return;
            }
        }
        Debug.LogWarning($"{machineName} 이름의 기계를 찾을 수 없습니다.");
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

    public bool _isPurchased = false;
    public bool _isOnMap = false; // 맵에 배치 여부

    public ItemInitializer relatedItem;

    public TextMeshProUGUI priceText; // UI에 표시할 가격 텍스트

    // isPurchased 프로퍼티 (게터/세터)
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