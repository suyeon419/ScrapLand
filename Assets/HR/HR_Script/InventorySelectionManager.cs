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

    // 손에 들기
    public TextMeshProUGUI DebugText; //디버그용 텍스트
    public GameObject Handpos; //손에 들기 위치 오브젝트

    // 인벤토리 매니저 딕셔너리 선언
    private Dictionary<string, Inventory> inventoryManager = new Dictionary<string, Inventory>();

    // HotBar, PlayerInventory 등 인벤토리 UI 매니저를 에디터에서 할당
    public InventoryUIManager hotBarUIManager;
    public InventoryUIManager playerInventoryUIManager;
    public InventoryController inventoryController; // 인벤토리 컨트롤러

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
            Debug.Log("HotBar 인벤토리 등록됨");
        }
        else
        {
            Debug.LogWarning("hotBarUIManager가 할당되지 않았습니다.");
        }
        if (playerInventoryUIManager != null)
        {
            inventoryManager["PlayerInventory"] = playerInventoryUIManager.GetInventory();
            Debug.Log("PlayerInventory 인벤토리 등록됨");
        }
        else
        {
            Debug.LogWarning("playerInventoryUIManager가 할당되지 않았습니다.");
        }

        Handpos.SetActive(false); // 시작 시 손에 들기 오브젝트 비활성화


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // P키로 인벤토리 아이템 데이터 출력
            GetInventoryItemData();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //RemoveItemFromAllInventories("Hat", 3);
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("SewingMachine");
        }
    }

    public static void SetSelection(GameObject slot, InventoryUIManager inventoryUIManager)
    {
        SelectedSlot = slot;
        SelectedInventoryUI = inventoryUIManager;

        InventoryItem item = slot.GetComponent<Slot>().GetItem();
        if (item != null)
        {
            int price = item.GetItemPrice(); // InventoryItem에서 가져온 가격
            ShopManager.Instance.ShopText.text = $"{price}코인 입니다. \n판매하시겠습니까?";
        }
        else
        {
            ShopManager.Instance.ShopText.text = $"아이템이 없습니다.";
        }

        Instance.OnSlotClicked();
    }

    public static void ClearSelection()
    {
        SelectedSlot = null;
        SelectedInventoryUI = null;
    }


    //해피포인트 관리

    // 아이템 타입 -> 판매 횟수
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

    //손에 들기
    public void OnSlotClicked()
    {
        if (SelectedSlot != null)
        {
            InventoryItem item = SelectedSlot.GetComponent<Slot>().GetItem();
            Sprite sprite = item.GetItemImage();
            if (item != null && !item.GetIsNull())
            {
                DebugText.text = $"선택된 아이템: {item.GetItemType()}";

                // 프리팹으로 들고 싶은 아이템 목록
                string[] prefabItems = {
                "Bench", "Can pot", "Clock", "Glass Pot", "Mobile", "Old Chest", "Plastic Pot",
                "T_Can", "T_Clothes", "T_Glass", "T_Paper", "T_Pet", "T_Plastic", "T_Vinyl", "Table"
            };
                string itemType = item.GetItemType();

                // 1. Handpos의 부모에 있는 프리팹(형제) 모두 삭제
                Transform parent = Handpos.transform.parent;
                if (parent != null)
                {
                    List<Transform> toDelete = new List<Transform>();
                    foreach (Transform child in parent)
                    {
                        if (child == Handpos.transform) continue;
                        if (System.Array.Exists(prefabItems, x => x == child.name))
                            toDelete.Add(child);
                    }
                    foreach (var t in toDelete)
                        Destroy(t.gameObject);
                }

                // 2. 프리팹 아이템이면 프리팹을 부모에 인스턴스화
                if (System.Array.Exists(prefabItems, x => x == itemType))
                {
                    Handpos.SetActive(false);
                    GameObject prefab = Resources.Load<GameObject>($"assets/Prefabs/{itemType}");
                    if (prefab != null)
                    {
                        GameObject go = Instantiate(prefab, Handpos.transform.position, Handpos.transform.rotation, Handpos.transform.parent);
                        go.name = itemType;
                        go.transform.localScale = Vector3.one * 0.5f; // 크기 조정 부분 원래대로 하고 싶으면 *1f
                    }
                    else
                    {
                        Debug.LogWarning($"{itemType} 프리팹을 찾을 수 없습니다. 경로를 확인하세요.");
                    }
                }
                // 3. 그 외에는 기존 방식대로 스프라이트를 머티리얼에 적용
                else if (sprite != null)
                {
                    Handpos.SetActive(true);
                    Material mat = Handpos.GetComponent<MeshRenderer>().material;
                    mat.mainTexture = sprite.texture;
                }
                else
                {
                    Handpos.SetActive(false);
                }
            }
            else
            {
                DebugText.text = "선택된 슬롯에 아이템이 없습니다.";
                Handpos.SetActive(false);

                // 빈 칸일 때도 프리팹 삭제
                string[] prefabItems = {
                "Bench", "Can pot", "Clock", "Glass Pot", "Mobile", "Old Chest", "Plastic Pot",
                "T_Can", "T_Clothes", "T_Glass", "T_Paper", "T_Pet", "T_Plastic", "T_Vinyl", "Table"};

                Transform parent = Handpos.transform.parent;
                if (parent != null)
                {
                    List<Transform> toDelete = new List<Transform>();
                    foreach (Transform child in parent)
                    {
                        if (child == Handpos.transform) continue;
                        if (System.Array.Exists(prefabItems, x => x == child.name))
                            toDelete.Add(child);
                    }
                    foreach (var t in toDelete)
                        Destroy(t.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("선택된 슬롯이 없습니다.");
        }
    }

    //조합대/제작대에 아이템 전달
    public void GetInventoryItemData()
    {
        InventoryData data = new InventoryData(inventoryManager);

        // 아이템별 총 개수 집계용 딕셔너리
        Dictionary<string, int> totalItemCounts = new Dictionary<string, int>();

        foreach (var inven in data.inventories)
        {
            string invenName = inven.Key; // 예: "HotBar", "PlayerInventory"
            foreach (var item in inven.Value)
            {
                // 아이템이 null이 아니고, amount가 1 이상일 때만 집계
                if (!string.IsNullOrEmpty(item.name) && item.amount > 0)
                {
                    if (totalItemCounts.ContainsKey(item.name))
                        totalItemCounts[item.name] += item.amount;
                    else
                        totalItemCounts[item.name] = item.amount;
                }
            }
        }

        // 최종 결과 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("==== 인벤토리 아이템 총합 ====");
        foreach (var pair in totalItemCounts)
        {
            sb.AppendLine($"{pair.Key}: {pair.Value}개");
        }
        Debug.Log(sb.ToString());
    }

    public int GetTotalItemCount(string itemName)
    {
        InventoryData data = new InventoryData(inventoryManager);
        int total = 0;
        foreach (var inven in data.inventories)
        {
            foreach (var item in inven.Value)
            {
                if (item.name == itemName)
                    total += item.amount;
            }
        }
        return total;
    }

    public void RemoveItemFromAllInventories(string itemType, int amount)
    {
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController가 할당되지 않았습니다.");
            return;
        }

        // HotBar에서 해당 아이템 개수 확인
        int hotBarCount = inventoryController.CountItems("HotBar", itemType);
        int toRemoveFromHotBar = Mathf.Min(hotBarCount, amount);

        // HotBar에서 먼저 삭제
        if (toRemoveFromHotBar > 0)
            inventoryController.RemoveItem("HotBar", itemType, toRemoveFromHotBar);

        // 남은 개수만큼 PlayerInventory에서 삭제
        int remaining = amount - toRemoveFromHotBar;
        if (remaining > 0)
            inventoryController.RemoveItem("PlayerInventory", itemType, remaining);
    }
}