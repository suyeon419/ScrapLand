using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;

public class InventorySelectionManager : MonoBehaviour
{
    public static InventorySelectionManager Instance;
    public static GameObject SelectedSlot { get; private set; }
    public static InventoryUIManager SelectedInventoryUI { get; private set; }

    // �տ� ���
    public TextMeshProUGUI DebugText; //����׿� �ؽ�Ʈ
    public GameObject Handpos; //�տ� ��� ��ġ ������Ʈ

    // �κ��丮 �Ŵ��� ��ųʸ� ����
    private Dictionary<string, Inventory> inventoryManager = new Dictionary<string, Inventory>();

    // HotBar, PlayerInventory �� �κ��丮 UI �Ŵ����� �����Ϳ��� �Ҵ�
    public InventoryUIManager hotBarUIManager;
    public InventoryUIManager playerInventoryUIManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (hotBarUIManager != null)
        {
            inventoryManager["HotBar"] = hotBarUIManager.GetInventory();
            Debug.Log("HotBar �κ��丮 ��ϵ�");
        }
        else
        {
            Debug.LogWarning("hotBarUIManager�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (playerInventoryUIManager != null)
        {
            inventoryManager["PlayerInventory"] = playerInventoryUIManager.GetInventory();
            Debug.Log("PlayerInventory �κ��丮 ��ϵ�");
        }
        else
        {
            Debug.LogWarning("playerInventoryUIManager�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // PŰ�� �κ��丮 ������ ������ ���
            GetInventoryItemData();
        }
    }

    public static void SetSelection(GameObject slot, InventoryUIManager inventoryUIManager)
    {
        SelectedSlot = slot;
        SelectedInventoryUI = inventoryUIManager;

        InventoryItem item = slot.GetComponent<Slot>().GetItem();
        if (item != null)
        {
            int price = item.GetItemPrice(); // InventoryItem���� ������ ����
            ShopManager.Instance.ShopText.text = $"{price}���� �Դϴ�. \n�Ǹ��Ͻðڽ��ϱ�?";
        }
        else
        {
            ShopManager.Instance.ShopText.text = $"�������� �����ϴ�.";
        }

        Instance.OnSlotClicked();
    }

    public static void ClearSelection()
    {
        SelectedSlot = null;
        SelectedInventoryUI = null;
    }


    //��������Ʈ ����

    // ������ Ÿ�� -> �Ǹ� Ƚ��
    private Dictionary<string, int> sellCounts = new Dictionary<string, int>();

    public int GetSellCount(string itemType)
    {
        if (sellCounts.TryGetValue(itemType, out int count))
            return count;
        return 0;
    }

    public void IncrementSellCount(string itemType)
    {
        if (sellCounts.ContainsKey(itemType))
            sellCounts[itemType]++;
        else
            sellCounts[itemType] = 1;
    }

    //�տ� ���
    public void OnSlotClicked()
    {
        if (SelectedSlot != null)
        {
            InventoryItem item = SelectedSlot.GetComponent<Slot>().GetItem();
            Sprite sprite = item.GetItemImage(); // InventoryItem���� ������ �̹���
            if (item != null && !item.GetIsNull())
            {
                DebugText.text = $"���õ� ������: {item.GetItemType()}";

                // 1. �������� ��Ƽ������ �ִٸ� �ٷ� ����
/*                if (item.itemMaterial != null)
                {
                    Handpos.GetComponent<MeshRenderer>().material = item.itemMaterial;
                }*/
                // 2. �������� ��������Ʈ�� �ִٸ�, �ؽ�ó�� ��ȯ�ؼ� ��Ƽ���� ����
                if (sprite != null)
                {
                    Material mat = Handpos.GetComponent<MeshRenderer>().material;
                    mat.mainTexture = sprite.texture;
                    // �ʿ��ϴٸ� mat.color = Color.white; �� �߰�
                }
                // 3. �����ۿ� �̹����� ������ �⺻ ��Ƽ�����
                else
                {
                    // �⺻ ��Ƽ����� �����ϰų�, ��Ȱ��ȭ ��
                }
            }
            else
            {
                DebugText.text = "���õ� ���Կ� �������� �����ϴ�.";
                // �������� ������ Handpos�� ���ų� �⺻ ��Ƽ�����
            }
        }
        else
        {
            Debug.Log("���õ� ������ �����ϴ�.");
        }
    }

    //���մ�/���۴뿡 ������ ����
    public void GetInventoryItemData()
    {
        InventoryData data = new InventoryData(inventoryManager);

        // �����ۺ� �� ���� ����� ��ųʸ�
        Dictionary<string, int> totalItemCounts = new Dictionary<string, int>();

        foreach (var inven in data.inventories)
        {
            string invenName = inven.Key; // ��: "HotBar", "PlayerInventory"
            foreach (var item in inven.Value)
            {
                // �������� null�� �ƴϰ�, amount�� 1 �̻��� ���� ����
                if (!string.IsNullOrEmpty(item.name) && item.amount > 0)
                {
                    if (totalItemCounts.ContainsKey(item.name))
                        totalItemCounts[item.name] += item.amount;
                    else
                        totalItemCounts[item.name] = item.amount;
                }
            }
        }

        // ���� ��� ���
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("==== �κ��丮 ������ ���� ====");
        foreach (var pair in totalItemCounts)
        {
            sb.AppendLine($"{pair.Key}: {pair.Value}��");
        }
        Debug.Log(sb.ToString());
    }
}
