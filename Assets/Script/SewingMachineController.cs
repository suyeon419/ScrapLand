using System.Collections.Generic;
using Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachineController : MonoBehaviour
{
    //������
    private int SewingMachine = 5;

    // ��� ������ ������ public ����(�������� �޾ƿ���)
    public int plasticThread = 0;  // ��Ʈ��
    public int paper = 0;         // ����
    public int plastic = 0;        // ���� �ö�ƽ
    public int oldCloth = 0;      // �� ��
    public int vinyl = 0;       //���

    // ���� ���¸� ������ ����
    public int CapMaking = 0;
    public int GroveMaking = 0;
    public int TopMaking = 0;
    public int BottomMaking = 0;
    public int ShoesMaking = 0;
    public int DollMaking = 0;

    // UI ��ư �� �ؽ�Ʈ (Inspector���� �Ҵ�)
    public Button capButton;       // ���� ���� ��ư
    public Button gloveButton;     // �尩 ���� ��ư
    public Button topButton;       // ���� ���� ��ư
    public Button bottomButton;    // ���� ���� ��ư
    public Button shoesButton;     // �Ź� ���� ��ư
    public Button dollButton;      // ���� ���� ��ư

    // ��ư ����Ʈ ����
    private List<Button> craftingButtons = new List<Button>();

    private GameObject sewingObject;

    //text UI
    [Header("����")]
    public TextMeshProUGUI capPt;
    public TextMeshProUGUI capPaper;
    public TextMeshProUGUI capMaking;
    [Header("�尩")]
    public TextMeshProUGUI glovePt;
    public TextMeshProUGUI gloveOldCloth;
    public TextMeshProUGUI gloveMaking;
    [Header("����")]
    public TextMeshProUGUI topPt;
    public TextMeshProUGUI topOldCloth;
    public TextMeshProUGUI topMaking;
    [Header("����")]
    public TextMeshProUGUI bottomPt;
    public TextMeshProUGUI bottomOldCloth;
    public TextMeshProUGUI bottomMaking;
    [Header("�Ź�")]
    public TextMeshProUGUI shoesOldCloth;
    public TextMeshProUGUI shoesPlastic;
    public TextMeshProUGUI shoesMaking;
    [Header("����")]
    public TextMeshProUGUI dollOldCloth;
    public TextMeshProUGUI dollPt;
    public TextMeshProUGUI dollVinyl;
    public TextMeshProUGUI dollMaking;

    // ���ۿ� �ʿ��� �ּ� ��� ����
    //����
    private int CapPt = 5;   // ���� ���ۿ� �ʿ��� �ּ� �ö�ƽ
    private int CapPaper = 1;     // ���� ���ۿ� �ʿ��� �ּ� ����
    //�尩
    private int GrovePt = 1;    // ���� ���ۿ� �ʿ��� �ּ� ��Ʈ��
    private int GloveOldCloth = 1;     // �尩 ���ۿ� �ʿ��� �ּ� �� ��
    //����
    private int TopPt = 1;
    private int TopOldCloth = 3;
    //����
    private int BottomPt = 1;
    private int BottomOldCloth = 3;
    //�Ź�
    private int ShoesPlastic = 2;
    private int ShoesOldCloth = 3;
    //����
    private int DollOldCloth = 2;
    private int DollPt = 2;
    private int DollVinyl = 1;

    void UpdateMaterialCountsFromInventory()
    {
        // ��ųʸ� �޾ƿ���
        //var itemCounts = InventorySelectionManager.Instance.GetTotalItemCounts();

        // �� ��� ������ �� �Ҵ� (������ 0)
        //plasticThread = itemCounts.ContainsKey("plasticThread") ? itemCounts["plasticThread"] : 0;
        //paper = itemCounts.ContainsKey("paper") ? itemCounts["paper"] : 0;
        //plastic = itemCounts.ContainsKey("plastic") ? itemCounts["plastic"] : 0;
        //oldCloth = itemCounts.ContainsKey("oldCloth") ? itemCounts["oldCloth"] : 0;
        //vinyl = itemCounts.ContainsKey("vinyl") ? itemCounts["vinyl"] : 0;
    }

    void Start()
    {
        UpdateMaterialCountsFromInventory();

        // ��ư Ŭ�� �̺�Ʈ ����
        capButton.onClick.AddListener(() => StartCapMaking());
        gloveButton.onClick.AddListener(() => StartGloveMaking());
        topButton.onClick.AddListener(() => StartTopMaking());
        topButton.onClick.AddListener(() => StartBottomMaking());
        shoesButton.onClick.AddListener(() => StartShoesMaking());
        dollButton.onClick.AddListener(() => StartDollMaking());

        // ��ư�� ����Ʈ�� �߰�
        craftingButtons.Add(capButton);
        craftingButtons.Add(gloveButton);
        craftingButtons.Add(topButton);
        craftingButtons.Add(bottomButton);
        craftingButtons.Add(shoesButton);
        craftingButtons.Add(dollButton);

        // �ʱ� ��ư ���� üũ
        UpdateButtonStates();
        UpdateUI();

    }

    void Update()
    {
        UpdateMaterialCountsFromInventory();
        //UpdateUI();
        //UpdateButtonStates();
    }

    // UI ������Ʈ �޼���
    void UpdateUI()
    {
        // ���� ���� ���� UI ������Ʈ
        capPt.text = plasticThread.ToString() + "/5";         
        capPaper.text = paper.ToString() + "/1";               
        capMaking.text = "���� Ƚ��: " + CapMaking.ToString();

        // �尩 ���� ���� UI ������Ʈ
        glovePt.text = plasticThread.ToString() + "/1";   
        gloveOldCloth.text = oldCloth.ToString() + "/1";     
        gloveMaking.text = "���� Ƚ��: " + GroveMaking.ToString();

        // ���� ���� ���� UI ������Ʈ
        topPt.text = plasticThread.ToString() + "/1";      
        topOldCloth.text = oldCloth.ToString() + "/3";    
        topMaking.text = "���� Ƚ��: " + TopMaking.ToString();

        // ���� ���� ���� UI ������Ʈ
        bottomPt.text = plasticThread.ToString() + "/1";      
        bottomOldCloth.text = oldCloth.ToString() + "/3";     
        bottomMaking.text = "���� Ƚ��: " + BottomMaking.ToString();

        // �Ź� ���� ���� UI ������Ʈ
        shoesPlastic.text = plastic.ToString() + "/2";
        shoesOldCloth.text = oldCloth.ToString() + "/3";
        shoesMaking.text = "���� Ƚ��: " + ShoesMaking.ToString();

        //���� ���� ���� UI ������Ʈ
        dollOldCloth.text = plastic.ToString() + "/2";
        dollPt.text = plasticThread.ToString() + "/2";
        dollVinyl.text = vinyl.ToString() + "/1";
    }

    // ��ư ���� ������Ʈ �޼���
    public void UpdateButtonStates()
    {
        // ���� ���� ��ư Ȱ��ȭ
        capButton.interactable = (plasticThread >= CapPt && paper >= CapPaper);

        // �尩 ���� ��ư Ȱ��ȭ
        gloveButton.interactable = (plasticThread >= GrovePt && oldCloth >= GloveOldCloth);

        //���� ���� ��ư Ȱ��ȭ
        topButton.interactable = (plasticThread >= TopPt && oldCloth >= TopOldCloth);

        //���� ���� ��ư Ȱ��ȭ
        bottomButton.interactable = (plasticThread >= BottomPt && oldCloth >= BottomOldCloth);

        //�Ź� ���� ��ư Ȱ��ȭ
        shoesButton.interactable = (plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth);

        //���� ���� ��ư Ȱ��ȭ
        dollButton.interactable = (oldCloth >= DollOldCloth && plasticThread >= DollPt && vinyl >= DollVinyl);
    }

    // ���� ���� ����
    void StartCapMaking()
    {
        if (plasticThread >= CapPt && paper >= CapPaper)
        {
            plasticThread -= CapPt;
            paper -= CapPaper;
            CapMaking++;
            SewingMachine--;
            Debug.Log("���� ���� ����! CapMaking: " + CapMaking);
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

    // �尩 ���� ����
    void StartGloveMaking()
    {
        if (plasticThread >= GrovePt && oldCloth >= GloveOldCloth)
        {
            plasticThread -= GrovePt;
            oldCloth -= GloveOldCloth;
            GroveMaking++;
            SewingMachine--;
            Debug.Log("�尩 ���� ����!");
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

    // ���� ���� ����
    void StartTopMaking()
    {
        if (plasticThread >= TopPt && oldCloth >= TopOldCloth)
        {
            plasticThread -= TopPt;
            oldCloth -= TopOldCloth;
            TopMaking++;
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

    // ���� ���� ����
    void StartBottomMaking()
    {
        if (plasticThread >= BottomPt && oldCloth >= BottomOldCloth)
        {
            plasticThread -= BottomPt;
            oldCloth -= BottomOldCloth;
            BottomMaking++;
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

    //�Ź� ���� ����
    void StartShoesMaking()
    {
        if(plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth)
        {
            plastic -= ShoesPlastic;
            oldCloth -= ShoesOldCloth;
            ShoesMaking++;
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

    //���� ���� ����
    void StartDollMaking()
    {
        if(oldCloth >= DollOldCloth && plasticThread >= DollPt && vinyl >= DollVinyl)
        {
            oldCloth -= DollOldCloth;
            plasticThread -= DollPt;
            vinyl -= DollVinyl;
            DollMaking++;
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