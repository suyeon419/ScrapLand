using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingController : MonoBehaviour
{
    //������ �ִ� ��� ����(�������� �޾ƿ���)
    private int can;             //ĵ
    private int paper;         // ����
    private int glass;       //����
    private int plasticBottle; //��Ʈ
    private int vinyl ;       //���
    private int plastic ;        // �ö�ƽ
    private int oldCloth;      // �� ��

    private int aluminum; // �˷�̴�
    private int compressedPaper; //��������
    private int moltenGlass; //���� ����
    private int plasticThread ;  // ��Ʈ��
    private int moltenPlastic ; //���� �ö�ƽ

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

    // ���� ���� ���� (private)
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

    // ����/���� (public)
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


    // ��ư �Ҵ�
    public Button bagButton;
    [Header("���׸������ ��ư")]
    public Button p_potButton;
    public Button g_potButton;
    public Button c_potButton;
    public Button tableButton;
    public Button chairButton;
    public Button storageBoxButton;
    public Button mobileButton;
    public Button clockButton;
    [Header("�Ǹſ���� ��ư")]
    public Button keyringButton;
    public Button tongsButton;
    public Button cupButton;
    public Button bowlButton;
    [Header("Ż �Ͱ��� ��ư")]
    public Button boatButton;
    [Header("������ ��ǰ���� ��ư")]
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
    [Header("�ö�ƽ ȭ��")]
    public TextMeshProUGUI potPt;
    private int PotPt = 1;
    public TextMeshProUGUI p_potMaking;

    [Header("ĵ ȭ��")]
    public TextMeshProUGUI potCan;
    private int PotCan = 1;
    public TextMeshProUGUI c_potMaking;

    [Header("���� ȭ��")]
    public TextMeshProUGUI potGlass;
    private int PopGlass = 1;
    public TextMeshProUGUI g_potMaking;

    [Header("���̺�")]
    public TextMeshProUGUI tableCompressedPaper;
    private int TableCompressedPaper = 4;
    public TextMeshProUGUI tableMaking;

    [Header("����")]
    public TextMeshProUGUI chairCompressedPaper;
    private int ChairCompressedPaper = 3;
    public TextMeshProUGUI chairMaking;

    [Header("������")]
    public TextMeshProUGUI storageBoxCompressedPaper;
    private int StorageBoxCompressedPaper = 2;
    public TextMeshProUGUI storageBoxCan;
    private int StorageBoxCan = 3;
    public TextMeshProUGUI storageBoxMaking;

    [Header("���")]
    public TextMeshProUGUI mobileMoltenGlass;
    private int MobileMoltenGlass = 1;
    public TextMeshProUGUI mobileVinyl;
    private int MobileVinyl = 3;
    public TextMeshProUGUI mobileCan;
    private int MobileCan = 1;
    public TextMeshProUGUI mobilePtThread;
    private int MobilePtThread = 2;
    public TextMeshProUGUI mobileMaking;

    [Header("�ð�")]
    public TextMeshProUGUI clockMoltenGlass;
    private int ClockMoltenGlass = 1;
    public TextMeshProUGUI clockCompressedPaper;
    private int ClockCompressedPaper = 2;
    public TextMeshProUGUI clockCan;
    private int ClockCan = 1;
    public TextMeshProUGUI clockMaking;

    [Header("Ű��")]
    public TextMeshProUGUI keyringMoltenPlastic;
    private int KeyringMoltenPlastic = 1;
    public TextMeshProUGUI keyringMaking;

    [Header("����")]
    public TextMeshProUGUI tongsAluminum;
    private int TongsAluminum = 2;
    public TextMeshProUGUI tongsMaking;

    [Header("��")]
    public TextMeshProUGUI cupCan;
    private int CupCan = 2;
    public TextMeshProUGUI cupMaking;

    [Header("�׸�")]
    public TextMeshProUGUI bowlMoltenGlass;
    private int BowlMoltenGlass = 2;
    public TextMeshProUGUI bowlMaking;

    [Header("��Ʈ����Ʈ")]
    public TextMeshProUGUI boatPaper;
    private int BoatPaper = 1;
    public TextMeshProUGUI boatCan;
    private int BoatCan = 2;
    public TextMeshProUGUI boatPt;
    private int BoatPt = 10;
    public TextMeshProUGUI boatMaking;

    [Header("������")]
    public TextMeshProUGUI frameAluminum;
    private int FrameAluminum = 10;
    public TextMeshProUGUI frameMaking;

    [Header("����")]
    public TextMeshProUGUI wheelMoltenPlastic;
    private int WheelMoltenPlastic = 5;
    public TextMeshProUGUI wheelCompressedPaper;
    private int WheelCompressedPaper = 5;
    public TextMeshProUGUI wheelMaking;

    [Header("ü��")]
    public TextMeshProUGUI chainAluminum;
    private int ChainAluminum = 5;
    public TextMeshProUGUI chainMaking;

    [Header("�ڵ�")]
    public TextMeshProUGUI handleMoltenPlastic;
    private int HandleMoltenPlastic = 3;
    public TextMeshProUGUI handleCompressedPaper;
    private int HandleCompressedPaper = 2;
    public TextMeshProUGUI handleMaking;

    [Header("�극��ũ")]
    public TextMeshProUGUI brakeCompressedPaper;
    private int BrakeCompressedPaper = 2;
    public TextMeshProUGUI brakePt;
    private int BrakePt = 3;
    public TextMeshProUGUI brakeMaking;

    [Header("����")]
    public TextMeshProUGUI saddleMoltenPlastic;
    private int SaddleMoltenPlasic = 2;
    public TextMeshProUGUI saddleOldCloth;
    private int SaddleOldCloth = 1;
    public TextMeshProUGUI saddleMaking;

    [Header("������")]
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
        //��ư Ŭ�� �̺�Ʈ
        bagButton.onClick.AddListener(() => StartBagMaking()); //����
        /*���׸���*/
        p_potButton.onClick.AddListener(() => StartP_PotMaking()); //�ö�ƽ ȭ��
        c_potButton.onClick.AddListener(() => StartC_PotMaking()); //ĵ ȭ��
        g_potButton.onClick.AddListener(() => StartG_PotMaking()); //���� ȭ��
        tableButton.onClick.AddListener(() => StartTableMaking()); //���̺�
        chairButton.onClick.AddListener(() => StartChairMaking()); //����
        storageBoxButton.onClick.AddListener(() => StartStorageBoxMaking()); //������
        mobileButton.onClick.AddListener(() => StartMobileMaking()); //���
        clockButton.onClick.AddListener(() => StartClockMaking()); //�ð�

        /*�Ǹſ�*/
        keyringButton.onClick.AddListener(() => StartKeyringMaking()); //Ű��
        tongsButton.onClick.AddListener(() => StartTongsMaking()); //����
        cupButton.onClick.AddListener(() => StartCupMaking()); //��
        bowlButton.onClick.AddListener(() => StartBowlMaking()); //�׸�

        /* Ż �� */
        boatButton.onClick.AddListener(() => StartBoatMaking()); //��Ʈ����Ʈ

        /*������ ��ǰ*/
        frameButton.onClick.AddListener(() => StartFrameMaking()); //������
        wheelButton.onClick.AddListener(() => StartWheelMaking()); //����
        chainButton.onClick.AddListener(() => StartChainMaking()); //ü��
        handleButton.onClick.AddListener(() => StartHandleMaking());//�ڵ�
        brakeButton.onClick.AddListener(() => StartBrakeMaking()); //�극��ũ
        saddleButton.onClick.AddListener(() => StartSaddleMaking()); //����

        /*������*/
        bikeButton.onClick.AddListener(() => StartBikeMaking()); //������

        // �ʱ� ��ư ���� üũ
        UpdateButtonStates();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterialCounts();
        UpdateUI();
        UpdateButtonStates();
    }

    //UI������Ʈ
    void UpdateUI()
    {
        bagOldCloth.text = oldCloth.ToString() + "/6";
        bagMaking.text = "���� Ƚ��: " + BagMaking.ToString();

        /* ���׸��� */
        //�ö�ƽ ȭ�� ����
        potPt.text = plasticBottle.ToString() + "/1";
        p_potMaking.text = "���� Ƚ��: " + P_PotMaiking.ToString();

        //ĵ ȭ�� ����
        potCan.text = can.ToString() + "/1";
        c_potMaking.text = "���� Ƚ��: " + C_PotMaiking.ToString();

        //���� ȭ�� ����
        potGlass.text = glass.ToString() + "/1";
        g_potMaking.text = "���� Ƚ��: " + G_PotMaiking.ToString();

        //���̺� ����
        tableCompressedPaper.text = compressedPaper.ToString() + "/4";
        tableMaking.text = "���� Ƚ��: " + TableMaking.ToString();

        //���� ����
        chairCompressedPaper.text = compressedPaper + "/3";
        chairMaking.text = "���� Ƚ��: " + ChairMaking.ToString();

        //������ ����
        storageBoxCompressedPaper.text = compressedPaper.ToString() + "/2";
        storageBoxCan.text = can.ToString() + "/3";
        storageBoxMaking.text = "���� Ƚ��: " + StorageBoxMaking.ToString();

        //��� ����
        mobileMoltenGlass.text = moltenGlass.ToString() + "/1";
        mobileVinyl.text = vinyl.ToString() + "/3";
        mobileCan.text = can.ToString() + "/1";
        mobilePtThread.text = plasticThread.ToString() + "/2";
        mobileMaking.text = "���� Ƚ��: " + MobileMaking.ToString();

        //�ð� ����
        clockMoltenGlass.text = moltenGlass.ToString() + "/1";
        clockCompressedPaper.text = compressedPaper.ToString() + "/2";
        clockCan.text = can.ToString() + "/1";
        clockMaking.text = "���� Ƚ��: " + ClockMaking.ToString();

        /* �Ǹſ� */
        //Ű�� ����
        keyringMoltenPlastic.text = moltenPlastic.ToString() + "/1";
        keyringMaking.text = "���� Ƚ��: " + KeyringMaking.ToString();

        //���� ����
        tongsAluminum.text = aluminum.ToString() + "/2";
        tongsMaking.text = "���� Ƚ��: " + TongsMaking.ToString();

        //�� ����
        cupCan.text = can.ToString() + "/2";
        cupMaking.text = "���� Ƚ��: " + CupMaking.ToString();

        //�׸� ����
        bowlMoltenGlass.text = moltenGlass.ToString() + "/2";
        bowlMaking.text = "���� Ƚ��: " + BowlMaking.ToString();

        /* Ż �� */
        //��Ʈ�� ��Ʈ
        boatPaper.text = paper.ToString() + "/1";
        boatCan.text = can.ToString() + "/2";
        boatPt.text = plasticBottle.ToString() + "/10";
        boatMaking.text = "���� Ƚ��: " + BoatMaking.ToString();

        /* ������ ��ǰ */
        //������
        frameAluminum.text = aluminum.ToString() + "/10";
        frameMaking.text = "���� Ƚ��: " + FrameMaking.ToString();

        //����
        wheelMoltenPlastic.text = moltenPlastic.ToString() + "/5";
        wheelCompressedPaper.text = compressedPaper.ToString() + "/5";
        wheelMaking.text = "���� Ƚ��: " + WheelMaking.ToString();

        //ü��
        chainAluminum.text = aluminum.ToString() + "/5";
        chainMaking.text = "���� Ƚ��: " + ChainMaking.ToString();

        //�ڵ�
        handleMoltenPlastic.text = moltenPlastic.ToString() + "/3";
        handleCompressedPaper.text = compressedPaper.ToString() + "/2";
        handleMaking.text = "���� Ƚ��: " + HandleMaking.ToString();

        //�극��ũ
        brakeCompressedPaper.text = compressedPaper.ToString() + "/2";
        brakePt.text = plasticThread.ToString() + "/3";
        brakeMaking.text = "���� Ƚ��: " + BrakeMaking.ToString();

        //����
        saddleMoltenPlastic.text = moltenPlastic.ToString() + "/2";
        saddleOldCloth.text = oldCloth.ToString() + "/1";
        saddleMaking.text = "���� Ƚ��: " + SaddleMaking.ToString();

        /* ������ */
        bikeFrame.text = frame.ToString() + "/1";
        bikeWheel.text = wheel.ToString() + "/1";
        bikeChain.text = chain.ToString() + "/1";
        bikeHandle.text = handle.ToString() + "/1";
        bikeBrake.text = brake.ToString() + "/1";
        bikeSaddle.text = saddle.ToString() + "/1";
        bikeMaking.text = "���� Ƚ��: " + BikeMaking.ToString();
    }

    //��ư ���� ������Ʈ
    void UpdateButtonStates()
    {
        bagButton.interactable = (oldCloth >= BagOldCloth);
        /* ���׸��� */
        p_potButton.interactable = (plasticBottle >= PotPt); //�ö�ƽ ȭ��
        c_potButton.interactable = (can >= PotCan); //ĵ ȭ��
        g_potButton.interactable = (glass >= PopGlass); //���� ȭ��
        tableButton.interactable = (compressedPaper >= TableCompressedPaper); //���̺�
        chairButton.interactable = (compressedPaper >= ChairCompressedPaper); //����
        storageBoxButton.interactable = (compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan); //������
        mobileButton.interactable = (moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread); //���
        clockButton.interactable = (moltenGlass >= ClockMoltenGlass && compressedPaper >= ClockCompressedPaper && can >= ClockCan); //�ð�

        /* �Ǹſ� */
        keyringButton.interactable = (moltenPlastic >= KeyringMoltenPlastic); //Ű��
        tongsButton.interactable = (aluminum >= TongsAluminum); //����
        cupButton.interactable = (can >= CupCan); //��
        bowlButton.interactable = (moltenGlass >= BowlMoltenGlass); //�׸�

        /* Ż �� */
        boatButton.interactable = (can >= BoatCan && paper >= BoatPaper && plasticBottle >= BoatPt); //��Ʈ�� ��Ʈ

        /* ������ ��� */
        frameButton.interactable = (aluminum >= FrameAluminum);//������
        wheelButton.interactable = (moltenPlastic >= WheelMoltenPlastic && compressedPaper >= WheelCompressedPaper); //����
        chainButton.interactable = (aluminum >= ChainAluminum); //ü��
        handleButton.interactable = (moltenPlastic >= HandleMoltenPlastic && compressedPaper >= HandleCompressedPaper);  //�ڵ�
        brakeButton.interactable = (compressedPaper >= BrakeCompressedPaper && plasticThread >= BrakePt); //�극��ũ
        saddleButton.interactable = (moltenPlastic >= SaddleMoltenPlasic && oldCloth >= SaddleOldCloth); //����

        /*������*/
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

    /* ������ */
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

    /* ������ ��� */
    //����
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

    //�극��ũ
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

    //�ڵ�
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

    //ü��
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

    //����
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

    //������
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

    /* Ż �� */
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

    /* �Ǹſ� */
    //�׸�
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

    //��
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

    //����
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

    //Ű��
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

    /* ���׸��� */
    //�ð�
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

   

    //���
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

    //������
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

    //����
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

    //���̺�
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

    //���� ȭ��
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

    //ĵ ȭ��
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

    //�ö�Ƽ ȭ�� ����
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
