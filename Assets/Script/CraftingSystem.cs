using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class CraftMaterial
{
    public string materialName;   // 재료 이름
    public int currentAmount;     // 현재 보유량
    public TextMeshProUGUI text;  // 재료 UI 텍스트
}

[System.Serializable]
public class CraftableItem
{
    public string itemName;       // 제작 아이템 이름
    [System.Serializable]
    public struct MaterialRequirement
    {
        public CraftMaterial material; // 재료
        public int requiredAmount;     // 필요량
    }
    public List<MaterialRequirement> requiredMaterials; // 필요한 재료 리스트
    public Button craftButton;     // 제작 버튼
}

public class CraftingSystem : MonoBehaviour
{
    // 재료 데이터 (Inspector에서 설정)
    public CraftMaterial[] materials;

    // 제작 아이템 데이터 (Inspector에서 설정)
    public CraftableItem[] craftableItems;

    void Start()
    {
        // UI 및 버튼 상태 초기화
        UpdateUI();
        UpdateButtonStates();

        // 버튼 이벤트 연결
        for (int i = 0; i < craftableItems.Length; i++)
        {
            int index = i;
            craftableItems[i].craftButton.onClick.AddListener(() => CraftItem(index));
        }
    }

    // UI 업데이트
    void UpdateUI()
    {
        // 재료 UI 업데이트
        foreach (var material in materials)
        {
            int maxRequired = 0;
            foreach (var item in craftableItems)
            {
                foreach (var req in item.requiredMaterials)
                {
                    if (req.material.materialName == material.materialName)
                    {
                        maxRequired = Mathf.Max(maxRequired, req.requiredAmount);
                    }
                }
            }
            material.text.text = $"{material.materialName} {material.currentAmount}/{maxRequired}";
        }

        // 제작 아이템 UI 업데이트
        for (int i = 0; i < craftableItems.Length; i++)
        {
            foreach (var req in craftableItems[i].requiredMaterials)
            {
                req.material.text.text = $"{req.material.materialName} {req.material.currentAmount}/{req.requiredAmount}";
            }
        }
    }

    // 버튼 상태 업데이트
    void UpdateButtonStates()
    {
        for (int i = 0; i < craftableItems.Length; i++)
        {
            craftableItems[i].craftButton.interactable = CanCraftItem(craftableItems[i]);
        }
    }

    // 재료 인덱스 찾기
    int GetMaterialIndex(string materialName)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].materialName == materialName) return i;
        }
        return -1;
    }

    // 제작 가능 여부 확인
    bool CanCraftItem(CraftableItem item)
    {
        foreach (var req in item.requiredMaterials)
        {
            if (req.material.currentAmount < req.requiredAmount)
                return false;
        }
        return true;
    }

    // 제작 버튼 클릭 시 호출
    public void CraftItem(int index)
    {
        CraftableItem item = craftableItems[index];
        if (CanCraftItem(item))
        {
            // 재료 소모
            foreach (var req in item.requiredMaterials)
            {
                req.material.currentAmount -= req.requiredAmount;
            }
            Debug.Log($"{item.itemName} 제작 완료!");
        }

        // UI 및 버튼 상태 업데이트
        UpdateUI();
        UpdateButtonStates();
    }

    // 테스트용: 재료 추가
    public void AddMaterial(string materialName, int amount)
    {
        int index = GetMaterialIndex(materialName);
        if (index >= 0)
        {
            materials[index].currentAmount += amount;
        }

        UpdateUI();
        UpdateButtonStates();
    }
}