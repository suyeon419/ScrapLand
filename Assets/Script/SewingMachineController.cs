using System.Collections.Generic;
using Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachineController : MonoBehaviour
{
    //내구도
    private int _sewingMachine = 5;
    public int SewingMachine
    {
        get { return _sewingMachine; }
        set { _sewingMachine = value; }
    }

    // 재료 개수를 관리할 public 변수(혜리한테 받아오기)
    private int plasticThread = 100;// 페트실
    private int paper; // 종이
    private int plastic;        // 녹인 플라스틱
    private int oldCloth;      // 헌 옷
    private int vinyl;       //비닐

    // 제작 상태를 관리할 변수
    private int _capMaking = 0;
    public int CapMaking
    {
        get { return _capMaking; }
        set { _capMaking = value; }
    }

    private int _groveMaking = 0;
    public int GroveMaking
    {
        get { return _groveMaking; }
        set { _groveMaking = value; }
    }

    private int _topMaking = 0;
    public int TopMaking
    {
        get { return _topMaking; }
        set { _topMaking = value; }
    }

    private int _bottomMaking = 0;
    public int BottomMaking
    {
        get { return _bottomMaking; }
        set { _bottomMaking = value; }
    }

    private int _shoesMaking = 0;
    public int ShoesMaking
    {
        get { return _shoesMaking; }
        set { _shoesMaking = value; }
    }

    private int _dollMaking = 0;
    public int DollMaking
    {
        get { return _dollMaking; }
        set { _dollMaking = value; }
    }

    // UI 버튼 및 텍스트 (Inspector에서 할당)
    public Button capButton;       // 모자 제작 버튼
    public Button gloveButton;     // 장갑 제작 버튼
    public Button topButton;       // 상의 제작 버튼
    public Button bottomButton;    // 하의 제작 버튼
    public Button shoesButton;     // 신발 제작 버튼
    public Button dollButton;      // 인형 제작 버튼

    // 버튼 리스트 선언
    private List<Button> craftingButtons = new List<Button>();

    private GameObject sewingObject;

    //text UI
    [Header("모자")]
    public TextMeshProUGUI capPt;
    public TextMeshProUGUI capPaper;
    public TextMeshProUGUI capMaking;
    [Header("장갑")]
    public TextMeshProUGUI glovePt;
    public TextMeshProUGUI gloveOldCloth;
    public TextMeshProUGUI gloveMaking;
    [Header("상의")]
    public TextMeshProUGUI topPt;
    public TextMeshProUGUI topOldCloth;
    public TextMeshProUGUI topMaking;
    [Header("하의")]
    public TextMeshProUGUI bottomPt;
    public TextMeshProUGUI bottomOldCloth;
    public TextMeshProUGUI bottomMaking;
    [Header("신발")]
    public TextMeshProUGUI shoesOldCloth;
    public TextMeshProUGUI shoesPlastic;
    public TextMeshProUGUI shoesMaking;
    [Header("인형")]
    public TextMeshProUGUI dollOldCloth;
    public TextMeshProUGUI dollPt;
    public TextMeshProUGUI dollVinyl;
    public TextMeshProUGUI dollMaking;

    // 제작에 필요한 최소 재료 개수
    //모자
    private int CapPt = 5;   // 모자 제작에 필요한 최소 플라스틱
    private int CapPaper = 1;     // 모자 제작에 필요한 최소 종이
    //장갑
    private int GrovePt = 1;    // 모자 제작에 필요한 최소 페트실
    private int GloveOldCloth = 1;     // 장갑 제작에 필요한 최소 헌 옷
    //상의
    private int TopPt = 1;
    private int TopOldCloth = 3;
    //하의
    private int BottomPt = 1;
    private int BottomOldCloth = 3;
    //신발
    private int ShoesPlastic = 2;
    private int ShoesOldCloth = 3;
    //인형
    private int DollOldCloth = 2;
    private int DollPt = 2;
    private int DollVinyl = 1;

    private void Awake()
    {
        UpdateMaterialCounts();        
    }

    void Start()
    {
        // 버튼 클릭 이벤트 설정
        capButton.onClick.AddListener(() => StartCapMaking());
        gloveButton.onClick.AddListener(() => StartGloveMaking());
        topButton.onClick.AddListener(() => StartTopMaking());
        topButton.onClick.AddListener(() => StartBottomMaking());
        shoesButton.onClick.AddListener(() => StartShoesMaking());
        dollButton.onClick.AddListener(() => StartDollMaking());

        // 버튼을 리스트에 추가
        craftingButtons.Add(capButton);
        craftingButtons.Add(gloveButton);
        craftingButtons.Add(topButton);
        craftingButtons.Add(bottomButton);
        craftingButtons.Add(shoesButton);
        craftingButtons.Add(dollButton);

        // 초기 버튼 상태 체크
        UpdateButtonStates();
        UpdateUI();

    }

    void UpdateMaterialCounts()
    {
        if (InventorySelectionManager.Instance != null)
        {
            oldCloth = InventorySelectionManager.Instance.GetTotalItemCount("T_Clothes");
            plasticThread = InventorySelectionManager.Instance.GetTotalItemCount("PetRope");
            paper = InventorySelectionManager.Instance.GetTotalItemCount("T_Paper");
            plastic = InventorySelectionManager.Instance.GetTotalItemCount("MeltPla");
            vinyl = InventorySelectionManager.Instance.GetTotalItemCount("T_Vinyl");
        }
    }

    void Update()
    {
        UpdateMaterialCounts();
        UpdateUI();
        UpdateButtonStates();
    }

    // UI 업데이트 메서드
    void UpdateUI()
    {
        // 모자 제작 관련 UI 업데이트
        capPt.text = plasticThread.ToString() + "/5";         
        capPaper.text = paper.ToString() + "/1";               
        capMaking.text = "만든 횟수: " + CapMaking.ToString();

        // 장갑 제작 관련 UI 업데이트
        glovePt.text = plasticThread.ToString() + "/1";   
        gloveOldCloth.text = oldCloth.ToString() + "/1";     
        gloveMaking.text = "만든 횟수: " + GroveMaking.ToString();

        // 상의 제작 관련 UI 업데이트
        topPt.text = plasticThread.ToString() + "/1";      
        topOldCloth.text = oldCloth.ToString() + "/3";    
        topMaking.text = "만든 횟수: " + TopMaking.ToString();

        // 하의 제작 관련 UI 업데이트
        bottomPt.text = plasticThread.ToString() + "/1";      
        bottomOldCloth.text = oldCloth.ToString() + "/3";     
        bottomMaking.text = "만든 횟수: " + BottomMaking.ToString();

        // 신발 제작 관련 UI 업데이트
        shoesPlastic.text = plastic.ToString() + "/2";
        shoesOldCloth.text = oldCloth.ToString() + "/3";
        shoesMaking.text = "만든 횟수: " + ShoesMaking.ToString();

        //인형 제작 관령 UI 업데이트
        dollOldCloth.text = plastic.ToString() + "/2";
        dollPt.text = plasticThread.ToString() + "/2";
        dollVinyl.text = vinyl.ToString() + "/1";
    }

    // 버튼 상태 업데이트 메서드
    public void UpdateButtonStates()
    {
        // 모자 제작 버튼 활성화
        capButton.interactable = (plasticThread >= CapPt && paper >= CapPaper);

        // 장갑 제작 버튼 활성화
        gloveButton.interactable = (plasticThread >= GrovePt && oldCloth >= GloveOldCloth);

        //상의 제작 버튼 활성화
        topButton.interactable = (plasticThread >= TopPt && oldCloth >= TopOldCloth);

        //하의 제작 버튼 활성화
        bottomButton.interactable = (plasticThread >= BottomPt && oldCloth >= BottomOldCloth);

        //신발 제작 버튼 활성화
        shoesButton.interactable = (plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth);

        //인형 제작 버튼 활성화
        dollButton.interactable = (oldCloth >= DollOldCloth && plasticThread >= DollPt && vinyl >= DollVinyl);
    }

    // 모자 제작 시작
    void StartCapMaking()
    {
        if (plasticThread >= CapPt && paper >= CapPaper)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", CapPt);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Paper", CapPaper);
            
            CapMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Hat");

            SewingMachine--;
            Debug.Log("모자 제작 시작! CapMaking: " + CapMaking);
            UpdateUI();
        }
        UpdateButtonStates();
        if(SewingMachine == 0)
        {
            SetAllButtonsActive(false);
            sewingObject = GameObject.Find("sewing(Clone)");
            if (sewingObject != null)
            {
                Destroy(sewingObject);
                
                SewingMachine = 5;
            }
        }
    }

    // 장갑 제작 시작
    void StartGloveMaking()
    {
        if (plasticThread >= GrovePt && oldCloth >= GloveOldCloth)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", GrovePt);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", GloveOldCloth);

            GroveMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Glove");

            SewingMachine--;
            Debug.Log("장갑 제작 시작!");
            UpdateUI();
        }
        UpdateButtonStates();
        sewingObject = GameObject.Find("sewing(Clone)");
        if (sewingObject != null)
        {
            Destroy(sewingObject);
            SewingMachine = 5;
        }

    }

    // 상의 제작 시작
    void StartTopMaking()
    {
        if (plasticThread >= TopPt && oldCloth >= TopOldCloth)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", TopPt);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", TopOldCloth);

            TopMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Shirt");

            SewingMachine--;
            UpdateUI();
        }
        UpdateButtonStates();
        sewingObject = GameObject.Find("sewing(Clone)");
        if (sewingObject != null)
        {
            Destroy(sewingObject);
            SewingMachine = 5;
        }
    }

    // 하의 제작 시작
    void StartBottomMaking()
    {
        if (plasticThread >= BottomPt && oldCloth >= BottomOldCloth)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", BottomPt);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", BottomOldCloth);

            BottomMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Pants");

            SewingMachine--;
            UpdateUI();
        }
        UpdateButtonStates();
        sewingObject = GameObject.Find("sewing(Clone)");
        if (sewingObject != null)
        {
            Destroy(sewingObject);
            SewingMachine = 5;
        }
    }

    //신발 제작 시작
    void StartShoesMaking()
    {
        if(plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltPla", ShoesPlastic);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", ShoesOldCloth);

            ShoesMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Shoes");

            SewingMachine--;
            UpdateUI();
        }
        UpdateButtonStates();
        sewingObject = GameObject.Find("sewing(Clone)");
        if (sewingObject != null)
        {
            Destroy(sewingObject);
            SewingMachine = 5;
        }
    }

    //인형 제작 시작
    void StartDollMaking()
    {
        if(oldCloth >= DollOldCloth && plasticThread >= DollPt && vinyl >= DollVinyl)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", DollOldCloth);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", DollPt);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Vinyl", DollVinyl);

            DollMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Doll");

            SewingMachine--;
            UpdateUI();
        }
        UpdateButtonStates();
        sewingObject = GameObject.Find("sewing(Clone)");
        if (sewingObject != null)
        {
            Destroy(sewingObject);
            SewingMachine = 5;
        }
    }

    public void SetAllButtonsActive(bool isActive)
    {
        Debug.Log("SetAllButtonsActive called: " + isActive);
        foreach (Button button in craftingButtons)
        {
            button.interactable = isActive;
        }
    }
    
}