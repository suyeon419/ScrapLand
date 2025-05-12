using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachineController : MonoBehaviour
{
    // 재료 개수를 관리할 public 변수(혜리한테 받아오기)
    public int plasticThread = 0;  // 페트실
    public int paper = 0;         // 종이
    public int plastic = 0;        // 녹인 플라스틱
    public int oldCloth = 0;      // 헌 옷
    public int vinyl = 0;       //비닐

    // 제작 상태를 관리할 변수
    private int CapMaking = 0;
    private int GroveMaking = 0;
    private int TopMaking = 0;
    private int BottomMaking = 0;
    private int ShoesMaking = 0;
    private int DollMaking = 0;

    // UI 버튼 및 텍스트 (Inspector에서 할당)
    public Button capButton;       // 모자 제작 버튼
    public Button gloveButton;     // 장갑 제작 버튼
    public Button topButton;       // 상의 제작 버튼
    public Button bottomButton;    // 하의 제작 버튼
    public Button shoesButton;     // 신발 제작 버튼
    public Button dollButton;      // 인형 제작 버튼

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

    void Start()
    {
        // 버튼 클릭 이벤트 설정
        capButton.onClick.AddListener(() => StartCapMaking());
        gloveButton.onClick.AddListener(() => StartGloveMaking());
        topButton.onClick.AddListener(() => StartTopMaking());
        topButton.onClick.AddListener(() => StartBottomMaking());
        shoesButton.onClick.AddListener(() => StartShoesMaking());
        dollButton.onClick.AddListener(() => StartDollMaking());


        // 초기 버튼 상태 체크
        UpdateButtonStates();
    }

    void Update()
    {
        UpdateUI();
        UpdateButtonStates();
    }

    // UI 업데이트 메서드
    void UpdateUI()
    {
        // 모자 제작 관련 UI 업데이트
        capPt.text = "페트실\n" + plasticThread.ToString() + "/5";         
        capPaper.text = "종이\n" + paper.ToString() + "/1";               
        capMaking.text = "만든 횟수: " + CapMaking.ToString();

        // 장갑 제작 관련 UI 업데이트
        glovePt.text = "페트실\n" + plasticThread.ToString() + "/1";   
        gloveOldCloth.text = "헌 옷\n" + oldCloth.ToString() + "/1";     
        gloveMaking.text = "만든 횟수: " + GroveMaking.ToString();

        // 상의 제작 관련 UI 업데이트
        topPt.text = "페트실\n" + plasticThread.ToString() + "/1";      
        topOldCloth.text = "헌 옷\n" + oldCloth.ToString() + "/3";    
        topMaking.text = "만든 횟수: " + TopMaking.ToString();

        // 하의 제작 관련 UI 업데이트
        bottomPt.text = "페트실\n" + plasticThread.ToString() + "/1";      
        bottomOldCloth.text = "헌 옷\n" + oldCloth.ToString() + "/3";     
        bottomMaking.text = "만든 횟수: " + BottomMaking.ToString();

        // 신발 제작 관련 UI 업데이트
        shoesPlastic.text = "녹인\n플라스틱\n" + plastic.ToString() + "/2";
        shoesOldCloth.text = "헌 옷\n" + oldCloth.ToString() + "/3";
        shoesMaking.text = "만든 횟수: " + ShoesMaking.ToString();

        //인형 제작 관령 UI 업데이트
        dollOldCloth.text = "헌 옷\n" + plastic.ToString() + "/2";
        dollPt.text = "페트실\n" + plasticThread.ToString() + "/2";
        dollVinyl.text = "비닐\n" + vinyl.ToString() + "/1";
    }

    // 버튼 상태 업데이트 메서드
    void UpdateButtonStates()
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
            plasticThread -= CapPt;
            paper -= CapPaper;
            CapMaking++;
            Debug.Log("모자 제작 시작! CapMaking: " + CapMaking);
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // 장갑 제작 시작
    void StartGloveMaking()
    {
        if (plasticThread >= GrovePt && oldCloth >= GloveOldCloth)
        {
            plasticThread -= GrovePt;
            oldCloth -= GloveOldCloth;
            GroveMaking++;
            Debug.Log("장갑 제작 시작!");
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // 상의 제작 시작
    void StartTopMaking()
    {
        if (plasticThread >= TopPt && oldCloth >= TopOldCloth)
        {
            plasticThread -= TopPt;
            oldCloth -= TopOldCloth;
            TopMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // 하의 제작 시작
    void StartBottomMaking()
    {
        if (plasticThread >= BottomPt && oldCloth >= BottomOldCloth)
        {
            plasticThread -= BottomPt;
            oldCloth -= BottomOldCloth;
            BottomMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //신발 제작 시작
    void StartShoesMaking()
    {
        if(plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth)
        {
            plastic -= ShoesPlastic;
            oldCloth -= ShoesOldCloth;
            ShoesMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //인형 제작 시작
    void StartDollMaking()
    {
        if(oldCloth >= DollOldCloth && plasticThread >= DollPt && vinyl >= DollVinyl)
        {
            oldCloth -= DollOldCloth;
            plasticThread -= DollPt;
            vinyl -= DollVinyl;
            DollMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }
    
}