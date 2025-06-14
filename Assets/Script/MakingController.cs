using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingController : MonoBehaviour
{
    //가지고 있는 재료 개수(혜리한테 받아오기)
    private int can;             //캔
    private int paper;         // 종이
    private int glass;       //유리
    private int plasticBottle; //페트
    private int vinyl ;       //비닐
    private int plastic ;        // 플라스틱
    private int oldCloth;      // 헌 옷

    private int aluminum; // 알루미늄
    private int compressedPaper; //압축종이
    private int moltenGlass; //녹은 유리
    private int plasticThread ;  // 페트실
    private int moltenPlastic ; //녹은 플라스틱

    private int frame ;
    private int wheel ;
    private int chain ;
    private int handle;
    private int brake ;
    private int saddle;

    void UpdateMaterialCounts()
    {
        if (InventorySelectionManager.Instance != null)
        {
            can = InventorySelectionManager.Instance.GetTotalItemCount("T_Can");
            paper = InventorySelectionManager.Instance.GetTotalItemCount("T_Paper");
            glass = InventorySelectionManager.Instance.GetTotalItemCount("T_Glass");
            plasticBottle = InventorySelectionManager.Instance.GetTotalItemCount("T_Pet");
            vinyl = InventorySelectionManager.Instance.GetTotalItemCount("T_Vinyl");
            plastic = InventorySelectionManager.Instance.GetTotalItemCount("T_Plastic");
            oldCloth = InventorySelectionManager.Instance.GetTotalItemCount("T_Clothes");

            aluminum = InventorySelectionManager.Instance.GetTotalItemCount("Al");
            compressedPaper = InventorySelectionManager.Instance.GetTotalItemCount("PressedPaper");
            moltenGlass = InventorySelectionManager.Instance.GetTotalItemCount("MeltGlass");
            plasticThread = InventorySelectionManager.Instance.GetTotalItemCount("PetRope");
            moltenPlastic = InventorySelectionManager.Instance.GetTotalItemCount("MeltPla");

            frame = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Frame");
            wheel = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Wheel");
            chain = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Chain");
            handle = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Handle");
            brake = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Brake");
            saddle = InventorySelectionManager.Instance.GetTotalItemCount("Bicycle_Saddle");
        }
    }

    // 내부 변수 선언 (private)
    private int _bagMaking = 0;
    private int _pPotMaking = 0;
    private int _gPotMaking = 0;
    private int _cPotMaking = 0;
    private int _tableMaking = 0;
    private int _chairMaking = 0;
    private int _storageBoxMaking = 0;
    private int _mobileMaking = 0;
    private int _clockMaking = 0;
    private int _keyringMaking = 0;
    private int _tongsMaking = 0;
    private int _cupMaking = 0;
    private int _bowlMaking = 0;
    private int _boatMaking = 0;
    private int _frameMaking = 0;
    private int _wheelMaking = 0;
    private int _chainMaking = 0;
    private int _handleMaking = 0;
    private int _brakeMaking = 0;
    private int _saddleMaking = 0;
    private int _bikeMaking = 0;

    // 게터/세터 (public)
    public int BagMaking
    {
        get { return _bagMaking; }
        set { _bagMaking = value; }
    }
    public int P_PotMaiking
    {
        get { return _pPotMaking; }
        set { _pPotMaking = value; }
    }
    public int G_PotMaiking
    {
        get { return _gPotMaking; }
        set { _gPotMaking = value; }
    }
    public int C_PotMaiking
    {
        get { return _cPotMaking; }
        set { _cPotMaking = value; }
    }
    public int TableMaking
    {
        get { return _tableMaking; }
        set { _tableMaking = value; }
    }
    public int ChairMaking
    {
        get { return _chairMaking; }
        set { _chairMaking = value; }
    }
    public int StorageBoxMaking
    {
        get { return _storageBoxMaking; }
        set { _storageBoxMaking = value; } 
    }
    public int MobileMaking
    {
        get { return _mobileMaking; }
        set { _mobileMaking = value; }
    }
    public int ClockMaking
    {
        get { return _clockMaking; }
        set { _clockMaking = value; }
    }
    public int KeyringMaking
    {
        get { return _keyringMaking; }
        set { _keyringMaking = value; }
    }
    public int TongsMaking
    {
        get { return _tongsMaking; }
        set { _tongsMaking = value; }
    }
    public int CupMaking
    {
        get { return _cupMaking; }
        set { _cupMaking = value; }
    }
    public int BowlMaking
    {
        get { return _bowlMaking; }
        set { _bowlMaking = value; }
    }
    public int BoatMaking
    {
        get { return _boatMaking; }
        set { _boatMaking = value; }
    }
    public int FrameMaking
    {
        get { return _frameMaking; }
        set { _frameMaking = value; }
    }
    public int WheelMaking
    {
        get { return _wheelMaking; }
        set { _wheelMaking = value; }
    }
    public int ChainMaking
    {
        get { return _chainMaking; }
        set { _chainMaking = value; }
    }
    public int HandleMaking
    {
        get { return _handleMaking; }
        set { _handleMaking = value; }
    }
    public int BrakeMaking
    {
        get { return _brakeMaking; }
        set { _brakeMaking = value; }
    }
    public int SaddleMaking
    {
        get { return _saddleMaking; }
        set { _saddleMaking = value; }
    }
    public int BikeMaking
    {
        get { return _bikeMaking; }
        set { _bikeMaking = value; }
    }


    // 버튼 할당
    public Button bagButton;
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
    public TextMeshProUGUI bagOldCloth;
    private int BagOldCloth = 6;
    public TextMeshProUGUI bagMaking;
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

    
    private void Awake()
    {
        UpdateMaterialCounts();
    }
    void Start()
    {
        //버튼 클릭 이벤트
        bagButton.onClick.AddListener(() => StartBagMaking()); //가방
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
        UpdateMaterialCounts();
        UpdateUI();
        UpdateButtonStates();
    }

    //UI업데이트
    void UpdateUI()
    {
        bagOldCloth.text = oldCloth.ToString() + "/6";
        bagMaking.text = "만든 횟수: " + BagMaking.ToString();

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
        bagButton.interactable = (oldCloth >= BagOldCloth);
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

    void StartBagMaking()
    {
        if(oldCloth >= BagOldCloth)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", BagOldCloth);

            BagMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Bag");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* 자전거 */
    void StartBikeMaking()
    {
        if(frame >= BikeFrame && wheel >= BikeWheel && chain >= BikeChain && handle >= BikeHandle && brake >= BikeBrake && saddle >= BikeFrame)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Frame", BikeFrame);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Wheel", BikeWheel);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Chain", BikeChain);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Handle", BikeHandle);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Brake", BikeBrake);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Bicycle_Saddle", BikeSaddle);

            BikeMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Bicycle");
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
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltPla", SaddleMoltenPlasic);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Clothes", SaddleOldCloth);

            SaddleMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Saddle");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //브레이크
    void StartBrakeMaking()
    {
        if(compressedPaper >= BrakeCompressedPaper && plasticThread >= BrakePt)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", BrakeCompressedPaper);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", BrakePt);

            BrakeMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Brake");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //핸들
    void StartHandleMaking()
    {
        if(moltenPlastic >= HandleMoltenPlastic && compressedPaper >= HandleCompressedPaper)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltPla", HandleMoltenPlastic);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", HandleCompressedPaper);

            HandleMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Handle");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //체인
    void StartChainMaking()
    {
        if(aluminum >= ChainAluminum)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Al", ChainAluminum);

            ChainMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Chain");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //바퀴
    void StartWheelMaking()
    {
        if(moltenPlastic >= WheelMoltenPlastic && compressedPaper >= WheelCompressedPaper)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltPla", WheelMoltenPlastic);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", WheelCompressedPaper);

            WheelMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Wheel");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //프레임
    void StartFrameMaking()
    {
        if(aluminum >= FrameAluminum)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Al", FrameAluminum);

            FrameMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("BiCycle_Frame");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* 탈 것 */
    void StartBoatMaking()
    {
        if(can >= BoatCan && paper >= BoatPaper && plasticBottle >= BoatPt)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", BoatCan);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Paper", BoatPaper);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", BrakePt);

            BoatMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Pet_Boat");

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
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltGlass", BowlMoltenGlass);

            BowlMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Bowl");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //컵
    void StartCupMaking()
    {
        if(can >= CupCan)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", CupCan);

            CupMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Cup");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //집게
    void StartTongsMaking()
    {
        if(aluminum >= TongsAluminum)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("Al", TongsAluminum);

            TongsMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Tongs");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //키링
    void StartKeyringMaking()
    {
        if (moltenPlastic >= KeyringMoltenPlastic)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltPla", KeyringMoltenPlastic);

            KeyringMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Keyring");

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
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltGlass", ClockMoltenGlass);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", ClockCompressedPaper);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", ClockCan);

            ClockMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Clock");

            UpdateUI();
        }
        UpdateButtonStates();
    }

   

    //모빌
    void StartMobileMaking()
    {
        if(moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("MeltGlass", MobileMoltenGlass);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Vinyl", MobileVinyl);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", MobileCan);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PerRope", MobilePtThread);

            MobileMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Mobile");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //수납함
    void StartStorageBoxMaking()
    {
        if(compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", StorageBoxCompressedPaper);
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", StorageBoxCan);

            StorageBoxMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Old Chest");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //의자
    void StartChairMaking()
    {
        if(compressedPaper >= ChairCompressedPaper)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", ChairCompressedPaper);

            ChairMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Bench");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //테이블
    void StartTableMaking()
    {
        if(compressedPaper >= TableCompressedPaper)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("PressedPaper", TableCompressedPaper);

            TableMaking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Table");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //유리 화분
    void StartG_PotMaking()
    {
        if(glass >= PopGlass)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Glass", PopGlass);

            G_PotMaiking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Glass Pot");
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //캔 화분
    void StartC_PotMaking()
    {
        if(can >= PotCan)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Can", PotCan);

            C_PotMaiking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Can Pot");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    //플라스티 화분 제작
    void StartP_PotMaking()
    {
        if(plasticBottle >= PotPt)
        {
            InventorySelectionManager.Instance.RemoveItemFromAllInventories("T_Pet", PotPt);

            P_PotMaiking++;
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Plastic Pot");

            UpdateUI();
        }
        UpdateButtonStates();
    }

    
}
