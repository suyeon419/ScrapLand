using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySelectionManager : MonoBehaviour
{
    public static InventorySelectionManager Instance;
    public static GameObject SelectedSlot { get; private set; }
    public static InventoryUIManager SelectedInventoryUI { get; private set; }

    // 손에 들기
    public TextMeshProUGUI DebugText; //디버그용 텍스트
    public GameObject Handpos; //손에 들기 위치 오브젝트

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
            Sprite sprite = item.GetItemImage(); // InventoryItem에서 가져온 이미지
            if (item != null && !item.GetIsNull())
            {
                DebugText.text = $"선택된 아이템: {item.GetItemType()}";

                // 1. 아이템의 머티리얼이 있다면 바로 적용
/*                if (item.itemMaterial != null)
                {
                    Handpos.GetComponent<MeshRenderer>().material = item.itemMaterial;
                }*/
                // 2. 아이템의 스프라이트만 있다면, 텍스처로 변환해서 머티리얼에 적용
                if (sprite != null)
                {
                    Material mat = Handpos.GetComponent<MeshRenderer>().material;
                    mat.mainTexture = sprite.texture;
                    // 필요하다면 mat.color = Color.white; 등 추가
                }
                // 3. 아이템에 이미지가 없으면 기본 머티리얼로
                else
                {
                    // 기본 머티리얼로 변경하거나, 비활성화 등
                }
            }
            else
            {
                DebugText.text = "선택된 슬롯에 아이템이 없습니다.";
                // 아이템이 없으니 Handpos를 비우거나 기본 머티리얼로
            }
        }
        else
        {
            Debug.Log("선택된 슬롯이 없습니다.");
        }
    }
}
