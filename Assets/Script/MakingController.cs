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

    public int frame = 0;
    public int wheel = 0;
    public int chain = 0;
    public int handle = 0;
    public int brake = 0;
    public int saddle = 0;

    //제작 상태
    /*인테리어*/
    public int P_PotMaiking = 0;
    public int G_PotMaiking = 0;
    public int C_PotMaiking = 0;
    public int TableMaking = 0;
    public int ChairMaking = 0;
    public int StorageBoxMaking = 0;
    public int MobileMaking = 0;
    public int ClockMaking = 0;
    /*판매용*/
    public int KeyringMaking = 0;
    public int TongsMaking = 0;
    public int CupMaking = 0;
    public int BowlMaking = 0;
    /*탈 것*/
    public int BoatMaking = 0;
    /* 자전거 부품 */
    public int FrameMaking = 0;
    public int WheelMaking = 0;
    public int ChainMaking = 0;
    public int HandleMaking = 0;
    public int BrakeMaking = 0;
    public int SaddleMaking = 0;
    //자전거
    public int BikeMaking = 0;

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
    [Header("탈 것관련 버튼")]
    public Button boatButton;
    [Header("자전거 부품관련 버튼")]
    public Button frameButton;
    public Button wheelButton;
    public Button chainButton;
    public Button handleButton;
    public Button brakeButton;
    public Button saddleButton;
    public Button bikeButton;

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

    [Header("페트병보트")]
    public TextMeshProUGUI boatPaper;
    private int BoatPaper = 1;
    public TextMeshProUGUI boatCan;
    private int BoatCan = 2;
    public TextMeshProUGUI boatPt;
    private int BoatPt = 10;
    public TextMeshProUGUI boatMaking;

    [Header("프레임")]
    public TextMeshProUGUI frameAluminum;
    private int FrameAluminum = 10;
    public TextMeshProUGUI frameMaking;

    [Header("바퀴")]
    public TextMeshProUGUI wheelMoltenPlastic;
    private int WheelMoltenPlastic = 5;
    public TextMeshProUGUI wheelCompressedPaper;
    private int WheelCompressedPaper = 5;
    public TextMeshProUGUI wheelMaking;

    [Header("체인")]
    public TextMeshProUGUI chainAluminum;
    private int ChainAluminum = 5;
    public TextMeshProUGUI chainMaking;

    [Header("핸들")]
    public TextMeshProUGUI handleMoltenPlastic;
    private int HandleMoltenPlastic = 3;
    public TextMeshProUGUI handleCompressedPaper;
    private int HandleCompressedPaper = 2;
    public TextMeshProUGUI handleMaking;

    [Header("브레이크")]
    public TextMeshProUGUI brakeCompressedPaper;
    private int BrakeCompressedPaper = 2;
    public TextMeshProUGUI brakePt;
    private int BrakePt = 3;
    public TextMeshProUGUI brakeMaking;

    [Header("안장")]
    public TextMeshProUGUI saddleMoltenPlastic;
    private int SaddleMoltenPlasic = 2;
    public TextMeshProUGUI saddleOldCloth;
    private int SaddleOldCloth = 1;
    public TextMeshProUGUI saddleMaking;

    [Header("자전거")]
    public TextMeshProUGUI bikeFrame;
    private int BikeFrame = 1;
    public TextMeshProUGUI bikeWheel;
    private int BikeWheel = 1;
    public TextMeshProUGUI bikeChain;
    private int BikeChain = 1;
    public TextMeshProUGUI bikeHandle;
    private int BikeHandle = 1;
    public TextMeshProUGUI bikeBrake;
    private int BikeBrake = 1;
    public TextMeshProUGUI bikeSaddle;
    private int BikeSaddle = 1;
    public TextMeshProUGUI bikeMaking;

    /*
    public void setMaking(string itemName, int itemMaking)
    {
        P_PotMaiking = GameManager_ScrapLand.instance.GetGount_Produce("Plastic Pot");
    }
    public void getMaking(string itemName)
    {
        GameManager_ScrapLand.instance.SetCount_Produce("Plastic Pot", P_PotMaiking);
    }
    */

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

        /* 탈 것 */
        boatButton.onClick.AddListener(() => StartBoatMaking()); //페트병보트

        /*자전거 부품*/
        frameButton.onClick.AddListener(() => StartFrameMaking()); //프레임
        wheelButton.onClick.AddListener(() => StartWheelMaking()); //바퀴
        chainButton.onClick.AddListener(() => StartChainMaking()); //체인
        handleButton.onClick.AddListener(() => StartHandleMaking());//핸들
        brakeButton.onClick.AddListener(() => StartBrakeMaking()); //브레이크
        saddleButton.onClick.AddListener(() => StartSaddleMaking()); //안장

        /*자전거*/
        bikeButton.onClick.AddListener(() => StartBikeMaking()); //자전거

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
        potPt.text = plasticBottle.ToString() + "/1";
        p_potMaking.text = "만든 횟수: " + P_PotMaiking.ToString();

        //캔 화분 관련
        potCan.text = can.ToString() + "/1";
        c_potMaking.text = "만든 횟수: " + C_PotMaiking.ToString();

        //유리 화분 관련
        potGlass.text = glass.ToString() + "/1";
        g_potMaking.text = "만든 횟수: " + G_PotMaiking.ToString();

        //테이블 관련
        tableCompressedPaper.text = compressedPaper.ToString() + "/4";
        tableMaking.text = "만든 횟수: " + TableMaking.ToString();

        //의자 관련
        chairCompressedPaper.text = compressedPaper + "/3";
        chairMaking.text = "만든 횟수: " + ChairMaking.ToString();

        //수납함 관련
        storageBoxCompressedPaper.text = compressedPaper.ToString() + "/2";
        storageBoxCan.text = can.ToString() + "/3";
        storageBoxMaking.text = "만든 횟수: " + StorageBoxMaking.ToString();

        //모빌 관련
        mobileMoltenGlass.text = moltenGlass.ToString() + "/1";
        mobileVinyl.text = vinyl.ToString() + "/3";
        mobileCan.text = can.ToString() + "/1";
        mobilePtThread.text = plasticThread.ToString() + "/2";
        mobileMaking.text = "만든 횟수: " + MobileMaking.ToString();

        //시계 관련
        clockMoltenGlass.text = moltenGlass.ToString() + "/1";
        clockCompressedPaper.text = compressedPaper.ToString() + "/2";
        clockCan.text = can.ToString() + "/1";
        clockMaking.text = "만든 횟수: " + ClockMaking.ToString();

        /* 판매용 */
        //키링 관련
        keyringMoltenPlastic.text = moltenPlastic.ToString() + "/1";
        keyringMaking.text = "만든 횟수: " + KeyringMaking.ToString();

        //집게 관련
        tongsAluminum.text = aluminum.ToString() + "/2";
        tongsMaking.text = "만든 횟수: " + TongsMaking.ToString();

        //컵 관련
        cupCan.text = can.ToString() + "/2";
        cupMaking.text = "만든 횟수: " + CupMaking.ToString();

        //그릇 관련
        bowlMoltenGlass.text = moltenGlass.ToString() + "/2";
        bowlMaking.text = "만든 횟수: " + BowlMaking.ToString();

        /* 탈 것 */
        //페트병 보트
        boatPaper.text = paper.ToString() + "/1";
        boatCan.text = can.ToString() + "/2";
        boatPt.text = plasticBottle.ToString() + "/10";
        boatMaking.text = "만든 횟수: " + BoatMaking.ToString();

        /* 자전거 부품 */
        //프레임
        frameAluminum.text = aluminum.ToString() + "/10";
        frameMaking.text = "만든 횟수: " + FrameMaking.ToString();

        //바퀴
        wheelMoltenPlastic.text = moltenPlastic.ToString() + "/5";
        wheelCompressedPaper.text = compressedPaper.ToString() + "/5";
        wheelMaking.text = "만든 횟수: " + WheelMaking.ToString();

        //체인
        chainAluminum.text = aluminum.ToString() + "/5";
        chainMaking.text = "만든 횟수: " + ChainMaking.ToString();

        //핸들
        handleMoltenPlastic.text = moltenPlastic.ToString() + "/3";
        handleCompressedPaper.text = compressedPaper.ToString() + "/2";
        handleMaking.text = "만든 횟수: " + HandleMaking.ToString();

        //브레이크
        brakeCompressedPaper.text = compressedPaper.ToString() + "/2";
        brakePt.text = plasticThread.ToString() + "/3";
        brakeMaking.text = "만든 횟수: " + BrakeMaking.ToString();

        //안장
        saddleMoltenPlastic.text = moltenPlastic.ToString() + "/2";
        saddleOldCloth.text = oldCloth.ToString() + "/1";
        saddleMaking.text = "만든 횟수: " + SaddleMaking.ToString();

        /* 자전거 */
        bikeFrame.text = frame.ToString() + "/1";
        bikeWheel.text = wheel.ToString() + "/1";
        bikeChain.text = chain.ToString() + "/1";
        bikeHandle.text = handle.ToString() + "/1";
        bikeBrake.text = brake.ToString() + "/1";
        bikeSaddle.text = saddle.ToString() + "/1";
        bikeMaking.text = "만든 횟수: " + BikeMaking.ToString();
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

        /* 탈 것 */
        boatButton.interactable = (can >= BoatCan && paper >= BoatPaper && plasticBottle >= BoatPt); //페트병 보트

        /* 자전거 재료 */
        frameButton.interactable = (aluminum >= FrameAluminum);//프레임
        wheelButton.interactable = (moltenPlastic >= WheelMoltenPlastic && compressedPaper >= WheelCompressedPaper); //바퀴
        chainButton.interactable = (aluminum >= ChainAluminum); //체인
        handleButton.interactable = (moltenPlastic >= HandleMoltenPlastic && compressedPaper >= HandleCompressedPaper);  //핸들
        brakeButton.interactable = (compressedPaper >= BrakeCompressedPaper && plasticThread >= BrakePt); //브레이크
        saddleButton.interactable = (moltenPlastic >= SaddleMoltenPlasic && oldCloth >= SaddleOldCloth); //안장

        /*자전거*/
        bikeButton.interactable = (frame >= BikeFrame && wheel >= BikeWheel && chain >= BikeChain && handle >= BikeHandle && brake >= BikeBrake && saddle >= BikeFrame);
    }

    /* 자전거 */
    void StartBikeMaking()
    {
        if(frame >= BikeFrame && wheel >= BikeWheel && chain >= BikeChain && handle >= BikeHandle && brake >= BikeBrake && saddle >= BikeFrame)
        {
            frame -= BikeFrame;
            wheel -= BikeWheel;
            chain -= BikeChain;
            handle -= BikeHandle;
            brake -= BikeBrake;
            saddle -= BikeSaddle;
            BikeMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* 자전거 재료 */
    //안장
    void StartSaddleMaking()
    {
        if(moltenPlastic >= SaddleMoltenPlasic && oldCloth >= SaddleOldCloth)
        {
            moltenPlastic -= SaddleMoltenPlasic;
            oldCloth -= SaddleOldCloth;
            SaddleMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //브레이크
    void StartBrakeMaking()
    {
        if(compressedPaper >= BrakeCompressedPaper && plasticThread >= BrakePt)
        {
            compressedPaper -= BrakeCompressedPaper;
            plasticThread -= BrakePt;
            BrakeMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //핸들
    void StartHandleMaking()
    {
        if(moltenPlastic >= HandleMoltenPlastic && compressedPaper >= HandleCompressedPaper)
        {
            moltenPlastic -= HandleMoltenPlastic;
            compressedPaper -= HandleCompressedPaper;
            HandleMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //체인
    void StartChainMaking()
    {
        if(aluminum >= ChainAluminum)
        {
            aluminum -= ChainAluminum;
            ChainMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //바퀴
    void StartWheelMaking()
    {
        if(moltenPlastic >= WheelMoltenPlastic && compressedPaper >= WheelCompressedPaper)
        {
            moltenPlastic -= WheelMoltenPlastic;
            compressedPaper -= WheelCompressedPaper;
            WheelMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //프레임
    void StartFrameMaking()
    {
        if(aluminum >= FrameAluminum)
        {
            aluminum -= FrameAluminum;
            FrameMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* 탈 것 */
    void StartBoatMaking()
    {
        if(can >= BoatCan && paper >= BoatPaper && plasticBottle >= BoatPt)
        {
            can -= BoatCan;
            paper -= BoatPaper;
            plasticBottle -= BoatPt;
            BoatMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
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
