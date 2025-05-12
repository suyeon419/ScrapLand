using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingController : MonoBehaviour
{
    //가지고 있는 재료 개수(혜리한테 받아오기)
    public int can = 0;             //캔
    public int paper = 0;         // 종이
    public int glass = 0;       //유리
    public int plasticBottle = 0; //페트
    public int vinyl = 0;       //비닐
    public int plastic = 0;        // 플라스틱
    public int oldCloth = 0;      // 헌 옷

    public int aluminum = 0; // 알루미늄
    public int compressedPaper = 0; //압축종이
    public int moltenGlass = 0; //녹은 유리
    public int plasticThread = 0;  // 페트실
    public int moltenPlastic = 0; //녹은 플라스틱

    //제작 상태
    /*인테리어*/
    private int P_PotMaiking = 0;
    private int G_PotMaiking = 0;
    private int C_PotMaiking = 0;
    private int TableMaking = 0;
    private int ChairMaking = 0;
    private int StorageBoxMaking = 0;
    private int MobileMaking = 0;
    private int ClockMaking = 0;
    /*판매용*/
    private int KeyringMaking = 0;
    private int TongsMaking = 0;
    private int CupMaking = 0;
    private int BowlMaking = 0;

    // 버튼 할당
    [Header("인테리어관련 버튼")]
    public Button p_potButton;
    public Button g_potButton;
    public Button c_potButton;
    public Button tableButton;
    public Button chairButton;
    public Button storageBoxButton;
    public Button mobileButton;
    public Button clockButton;
    [Header("판매용관련 버튼")]
    public Button keyringButton;
    public Button tongsButton;
    public Button cupButton;
    public Button bowlButton;

    //text UI
    [Header("플라스틱 화분")]
    public TextMeshProUGUI potPt;
    private int PotPt = 1;
    public TextMeshProUGUI p_potMaking;

    [Header("캔 화분")]
    public TextMeshProUGUI potCan;
    private int PotCan = 1;
    public TextMeshProUGUI c_potMaking;

    [Header("유리 화분")]
    public TextMeshProUGUI potGlass;
    private int PopGlass = 1;
    public TextMeshProUGUI g_potMaking;

    [Header("테이블")]
    public TextMeshProUGUI tableCompressedPaper;
    private int TableCompressedPaper = 4;
    public TextMeshProUGUI tableMaking;

    [Header("의자")]
    public TextMeshProUGUI chairCompressedPaper;
    private int ChairCompressedPaper = 3;
    public TextMeshProUGUI chairMaking;

    [Header("수납함")]
    public TextMeshProUGUI storageBoxCompressedPaper;
    private int StorageBoxCompressedPaper = 2;
    public TextMeshProUGUI storageBoxCan;
    private int StorageBoxCan = 3;
    public TextMeshProUGUI storageBoxMaking;

    [Header("모빌")]
    public TextMeshProUGUI mobileMoltenGlass;
    private int MobileMoltenGlass = 1;
    public TextMeshProUGUI mobileVinyl;
    private int MobileVinyl = 3;
    public TextMeshProUGUI mobileCan;
    private int MobileCan = 1;
    public TextMeshProUGUI mobilePtThread;
    private int MobilePtThread = 2;
    public TextMeshProUGUI mobileMaking;

    [Header("시계")]
    public TextMeshProUGUI clockMoltenGlass;
    private int ClockMoltenGlass = 1;
    public TextMeshProUGUI clockCompressedPaper;
    private int ClockCompressedPaper = 2;
    public TextMeshProUGUI clockCan;
    private int ClockCan = 1;
    public TextMeshProUGUI clockMaking;

    [Header("키링")]
    public TextMeshProUGUI keyringMoltenPlastic;
    private int KeyringMoltenPlastic = 1;
    public TextMeshProUGUI keyringMaking;

    [Header("집게")]
    public TextMeshProUGUI tongsAluminum;
    private int TongsAluminum = 2;
    public TextMeshProUGUI tongsMaking;

    [Header("컵")]
    public TextMeshProUGUI cupCan;
    private int CupCan = 2;
    public TextMeshProUGUI cupMaking;

    [Header("그릇")]
    public TextMeshProUGUI bowlMoltenGlass;
    private int BowlMoltenGlass = 2;
    public TextMeshProUGUI bowlMaking;


    // Start is called before the first frame update
    void Start()
    {
        //버튼 클릭 이벤트
        /*인테리어*/
        p_potButton.onClick.AddListener(() => StartP_PotMaking()); //플라스틱 화분
        c_potButton.onClick.AddListener(() => StartC_PotMaking()); //캔 화분
        g_potButton.onClick.AddListener(() => StartG_PotMaking()); //유리 화분
        tableButton.onClick.AddListener(() => StartTableMaking()); //테이블
        chairButton.onClick.AddListener(() => StartChairMaking()); //의자
        storageBoxButton.onClick.AddListener(() => StartStorageBoxMaking()); //수납함
        mobileButton.onClick.AddListener(() => StartMobileMaking()); //모빌
        clockButton.onClick.AddListener(() => StartClockMaking()); //시계

        /*판매용*/
        keyringButton.onClick.AddListener(() => StartKeyringMaking()); //키링
        tongsButton.onClick.AddListener(() => StartTongsMaking()); //집게
        cupButton.onClick.AddListener(() => StartCupMaking()); //컵
        bowlButton.onClick.AddListener(() => StartBowlMaking()); //그릇

        // 초기 버튼 상태 체크
        UpdateButtonStates();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        UpdateButtonStates();
    }

    //UI업데이트
    void UpdateUI()
    {
        /* 인테리어 */
        //플라스틱 화분 관련
        potPt.text = "페트\n" + plasticBottle.ToString() + "/1";
        p_potMaking.text = "만든 횟수: " + P_PotMaiking.ToString();

        //캔 화분 관련
        potCan.text = "캔\n" + can.ToString() + "/1";
        c_potMaking.text = "만든 횟수: " + C_PotMaiking.ToString();

        //유리 화분 관련
        potGlass.text = "유리\n" + glass.ToString() + "/1";
        g_potMaking.text = "만든 횟수: " + G_PotMaiking.ToString();

        //테이블 관련
        tableCompressedPaper.text = "압축 종이\n" + compressedPaper.ToString() + "/4";
        tableMaking.text = "만든 횟수: " + TableMaking.ToString();

        //의자 관련
        chairCompressedPaper.text = "압축 종이\n" + compressedPaper + "/3";
        chairMaking.text = "만든 횟수: " + ChairMaking.ToString();

        //수납함 관련
        storageBoxCompressedPaper.text = "압축 종이\n" + compressedPaper.ToString() + "/2";
        storageBoxCan.text = "캔\n" + can.ToString() + "/3";
        storageBoxMaking.text = "만든 횟수: " + StorageBoxMaking.ToString();

        //모빌 관련
        mobileMoltenGlass.text = "녹인 유리\n" + moltenGlass.ToString() + "/1";
        mobileVinyl.text = "비닐\n" + vinyl.ToString() + "/3";
        mobileCan.text = "캔\n" + can.ToString() + "/1";
        mobilePtThread.text = "페트실\n" + plasticThread.ToString() + "/2";
        mobileMaking.text = "만든 횟수: " + MobileMaking.ToString();

        //시계 관련
        clockMoltenGlass.text = "녹인 유리\n" + moltenGlass.ToString() + "/1";
        clockCompressedPaper.text = "압축 종이\n" + compressedPaper.ToString() + "/2";
        clockCan.text = "캔\n" + can.ToString() + "/1";
        clockMaking.text = "만든 횟수: " + ClockMaking.ToString();

        /* 판매용 */
        //키링 관련
        keyringMoltenPlastic.text = "녹인\n플라스틱\n" + moltenPlastic.ToString() + "/1";
        keyringMaking.text = "만든 횟수: " + KeyringMaking.ToString();

        //집게 관련
        tongsAluminum.text = "알루미늄\n" + aluminum.ToString() + "/2";
        tongsMaking.text = "만든 횟수: " + TongsMaking.ToString();

        //컵 관련
        cupCan.text = "캔\n" + can.ToString() + "/2";
        cupMaking.text = "만든 횟수: " + CupMaking.ToString();

        //그릇 관련
        bowlMoltenGlass.text = "녹인 유리\n" + moltenGlass.ToString() + "/2";
        bowlMaking.text = "만든 횟수: " + BowlMaking.ToString();
    }

    //버튼 상태 업데이트
    void UpdateButtonStates()
    {
        /* 인테리어 */
        p_potButton.interactable = (plasticBottle >= PotPt); //플라스틱 화분
        c_potButton.interactable = (can >= PotCan); //캔 화분
        g_potButton.interactable = (glass >= PopGlass); //유리 화분
        tableButton.interactable = (compressedPaper >= TableCompressedPaper); //테이블
        chairButton.interactable = (compressedPaper >= ChairCompressedPaper); //의자
        storageBoxButton.interactable = (compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan); //수납함
        mobileButton.interactable = (moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread); //모빌
        clockButton.interactable = (moltenGlass >= ClockMoltenGlass && compressedPaper >= ClockCompressedPaper && can >= ClockCan); //시계

        /* 판매용 */
        keyringButton.interactable = (moltenPlastic >= KeyringMoltenPlastic); //키링
        tongsButton.interactable = (aluminum >= TongsAluminum); //집게
        cupButton.interactable = (can >= CupCan); //컵
        bowlButton.interactable = (moltenGlass >= BowlMoltenGlass); //그릇

    }

    /* 판매용 */
    //그릇
    void StartBowlMaking()
    {
        if(moltenGlass >= BowlMoltenGlass)
        {
            moltenGlass -= BowlMoltenGlass;
            BowlMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //컵
    void StartCupMaking()
    {
        if(can >= CupCan)
        {
            can -= CupCan;
            CupMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //집게
    void StartTongsMaking()
    {
        if(aluminum >= TongsAluminum)
        {
            aluminum -= TongsAluminum;
            TongsMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //키링
    void StartKeyringMaking()
    {
        if (moltenPlastic >= KeyringMoltenPlastic)
        {
            moltenPlastic -= KeyringMoltenPlastic;
            KeyringMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* 인테리어 */
    //시계
    void StartClockMaking()
    {
        if(moltenGlass >= ClockMoltenGlass && compressedPaper >= ClockCompressedPaper && can >= ClockCan)
        {
            moltenGlass -= ClockMoltenGlass;
            compressedPaper -= ClockCompressedPaper;
            can -= ClockCan;
            ClockMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

   

    //모빌
    void StartMobileMaking()
    {
        if(moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread)
        {
            moltenGlass -= MobileMoltenGlass;
            vinyl -= MobileVinyl;
            can -= MobileCan;
            plasticThread -= MobilePtThread;
            MobileMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //수납함
    void StartStorageBoxMaking()
    {
        if(compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan)
        {
            compressedPaper -= StorageBoxCompressedPaper;
            can -= StorageBoxCan;
            StorageBoxMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //의자
    void StartChairMaking()
    {
        if(compressedPaper >= ChairCompressedPaper)
        {
            compressedPaper -= ChairCompressedPaper;
            ChairMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //테이블
    void StartTableMaking()
    {
        if(compressedPaper >= TableCompressedPaper)
        {
            compressedPaper -= TableCompressedPaper;
            TableMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //유리 화분
    void StartG_PotMaking()
    {
        if(glass >= PopGlass)
        {
            glass -= PopGlass;
            G_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //캔 화분
    void StartC_PotMaking()
    {
        if(can >= PotCan)
        {
            can -= PotCan;
            C_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //플라스티 화분 제작
    void StartP_PotMaking()
    {
        if(plasticBottle >= PotPt)
        {
            plasticBottle -= PotPt;
            P_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    
}
