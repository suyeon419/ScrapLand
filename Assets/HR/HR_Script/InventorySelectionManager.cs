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

        Handpos.SetActive(true);

        /*        // 핫바 슬롯 리스트 가져오기
                var slots = typeof(InventoryUIManager)
                    .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(hotBarUIManager) as List<GameObject>;

                if (slots != null && slots.Count > 0 && slots[0] != null)
                {
                    SetSelection(slots[0], hotBarUIManager);
                }
                else
                {
                    Debug.LogWarning("핫바 슬롯이 아직 초기화되지 않았습니다. Start() 이후에 슬롯이 생성될 수 있습니다.");
                }*/
        EnsureSelection();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //RemoveSelectedHotBarItem();
            AddItemToSelectedSlot("Hat");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            RemoveItemFromAllInventories("Hat", 3);
            //InventoryController.instance.AddItemPos("HotBar", "Hat", 5);
            //Debug.Log(CheckInvenFull());
/*            bool hasHotBarEmpty = InventoryController.instance.GetInventory("HotBar").HasEmptySlot();
            bool hasPlayerInventoryEmpty = InventoryController.instance.GetInventory("PlayerInventory").HasEmptySlot();
            Debug.Log($"핫바 빈 슬롯: {hasHotBarEmpty}, 플레이어 인벤토리 빈 슬롯: {hasPlayerInventoryEmpty}");*/

        }
    }

    public static void SetSelection(GameObject slot, InventoryUIManager inventoryUIManager)
    {
        /*        SelectedSlot = slot;
                SelectedInventoryUI = inventoryUIManager;*/

        if (slot == null || inventoryUIManager == null)
        {
            Debug.LogWarning("SetSelection: slot 또는 inventoryUIManager가 null입니다.");
            return;
        }

        // 이전 선택 슬롯 하이라이트 해제
        if (SelectedSlot != null && SelectedInventoryUI != null && SelectedSlot != slot)
        {
            SelectedInventoryUI.UnHighlight(SelectedSlot);
        }

        SelectedSlot = slot;
        SelectedInventoryUI = inventoryUIManager;

        // 새 선택 슬롯 하이라이트
        SelectedInventoryUI.Highlight(SelectedSlot);

        var slotComponent = slot.GetComponent<Slot>();
        if (slotComponent == null)
        {
            Debug.LogWarning("SetSelection: 선택된 슬롯에 Slot 컴포넌트가 없습니다.");
            return;
        }

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
        if (SelectedSlot == null)
        {
            Debug.Log("OnSlotClicked: 선택된 슬롯이 없습니다.");
            return;
        }

        var slotComponent = SelectedSlot.GetComponent<Slot>();
        if (slotComponent == null)
        {
            Debug.LogWarning("OnSlotClicked: 선택된 슬롯에 Slot 컴포넌트가 없습니다.");
            return;
        }

        InventoryItem item = slotComponent.GetItem();
        Sprite sprite = item != null ? item.GetItemImage() : null;


        if (SelectedSlot != null)
        {
/*            InventoryItem item = SelectedSlot.GetComponent<Slot>().GetItem();
            Sprite sprite = item.GetItemImage();*/

            // 인벤토리의 모든 아이템 타입 목록 동적 생성
            var itemTypeList = new HashSet<string>();
            foreach (var initializer in InventoryController.instance.items)
            {
                itemTypeList.Add(initializer.GetItemType());
            }

            // Handpos의 모든 하위 오브젝트 삭제 (무조건 1개만 유지)
            foreach (Transform child in Handpos.transform)
            {
                Destroy(child.gameObject);
            }

            if (item != null && !item.GetIsNull())
            {
                DebugText.text = $"선택된 아이템: {item.GetItemType()}";
                string itemType = item.GetItemType();

                PlacementManager.Instance.SetHeldItem(itemType);

                if (itemTypeList.Contains(itemType))
                {
                    GameObject prefab = Resources.Load<GameObject>($"assets/Prefabs/{itemType}");
                    if (prefab != null)
                    {
                        GameObject go = Instantiate(prefab, Handpos.transform);
                        go.name = itemType;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = Vector3.one * 0.5f; // 필요시 크기 조정
                    }
                    else
                    {
                        Debug.LogWarning($"{itemType} 프리팹을 찾을 수 없습니다. 경로를 확인하세요.");
                    }
                }
                else if (sprite != null)
                {
                    Material mat = Handpos.GetComponent<MeshRenderer>().material;
                    mat.mainTexture = sprite.texture;
                }
            }
            else
            {
                DebugText.text = "선택된 슬롯에 아이템이 없습니다.";
                // PlacementManager.Instance.SetHeldItem(null); // 필요시 주석 해제
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


        // 선택된 슬롯의 상태 갱신
        if (SelectedSlot != null && SelectedInventoryUI != null)
            SetSelection(SelectedSlot, SelectedInventoryUI);

    }

    public void RemoveSelectedHotBarItem(int amount = 1)
    {
        if (SelectedSlot == null || hotBarUIManager == null || inventoryController == null)
        {
            Debug.LogWarning("SelectedSlot, hotBarUIManager 또는 inventoryController가 할당되지 않았습니다.");
            return;
        }

        // 핫바 슬롯인지 확인
        // hotBarUIManager의 슬롯 리스트에 포함되어 있는지 체크
        bool isHotBarSlot = false;
        int slotIndex = -1;
        var slots = typeof(InventoryUIManager)
            .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(hotBarUIManager) as List<GameObject>;

        if (slots != null)
        {
            slotIndex = slots.IndexOf(SelectedSlot);
            isHotBarSlot = slotIndex >= 0;
        }

        if (!isHotBarSlot)
        {
            Debug.Log("선택된 슬롯이 핫바에 속하지 않습니다.");
            return;
        }

        InventoryItem item = SelectedSlot.GetComponent<Slot>().GetItem();
        if (item != null && !item.GetIsNull())
        {
            inventoryController.RemoveItemPos("HotBar", slotIndex, amount);
            Debug.Log($"핫바 {slotIndex}번 슬롯의 아이템 {amount}개 삭제 완료");
        }
        else
        {
            Debug.Log("선택된 슬롯에 아이템이 없습니다.");
        }

        // 선택된 슬롯의 상태 갱신
        if (SelectedSlot != null && SelectedInventoryUI != null)
            SetSelection(SelectedSlot, SelectedInventoryUI);

    }

    //현재 선택한 슬롯에 아이템 추가
    public void AddItemToSelectedSlot(string item)
    {
        // 핫바 슬롯 리스트 가져오기 (private 필드 접근)
        var slots = typeof(InventoryUIManager)
            .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(hotBarUIManager) as List<GameObject>;

        int slotIndex = -1;
        if (slots != null && SelectedSlot != null)
        {
            slotIndex = slots.IndexOf(SelectedSlot);
        }

        if (slotIndex >= 0)
        {
            InventoryController.instance.AddItemPos("HotBar", item, slotIndex, 1);
        }
        else
        {
            Debug.LogWarning("선택된 슬롯이 핫바에 없거나 슬롯 인덱스를 찾을 수 없습니다.");
        }

        OnSlotClicked(); // 슬롯 클릭 이벤트 호출하여 아이템 정보 업데이트
    }

    public bool CheckInvenFull()
    {
        bool isHotBarFull = !InventoryController.instance.GetInventory("HotBar").HasEmptySlot();
        bool isPlayerInventoryFull = !InventoryController.instance.GetInventory("PlayerInventory").HasEmptySlot();

        //Debug.Log($"핫바 가득 찼는지: {isHotBarFull}, 플레이어 인벤토리 가득 찼는지: {isPlayerInventoryFull}, 플레이어 인벤토리 활성화 여부: {PlayerInvenManager.instance.IsBagOn}");

        if (PlayerInvenManager.instance.IsBagOn)
        {
            // PlayerInventory가 활성화되어 있으면 둘 다 가득 찼을 때만 true
            return isHotBarFull && isPlayerInventoryFull;
        }
        else
        {
            // PlayerInventory가 비활성화면 핫바만 검사
            return isHotBarFull;
        }
    }

    //핫바 첫 칸 선택
    public void EnsureSelection()
    {
        if (SelectedSlot == null || SelectedInventoryUI == null)
        {
            var slots = typeof(InventoryUIManager)
                .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(hotBarUIManager) as List<GameObject>;

            if (slots != null && slots.Count > 0 && slots[0] != null && slots[0].GetComponent<Slot>() != null)
            {
                SetSelection(slots[0], hotBarUIManager);
            }
            else
            {
                Debug.LogWarning("EnsureSelection: 핫바 슬롯이 아직 초기화되지 않았거나 Slot 컴포넌트가 없습니다.");
            }
        }
    }
}