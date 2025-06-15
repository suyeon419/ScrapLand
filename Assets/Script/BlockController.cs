using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlockController : MonoBehaviour
{
    public static BlockController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public bool machine
    {
        get => ShopManager.Instance.machines[2].isPurchased;
        set
        {
            ShopManager.Instance.machines[2].isPurchased = value;
            UpdateAllUI();
        }
    }

    public bool blastFurnace
    {
        get => ShopManager.Instance.machines[4].isPurchased;
        set
        {
            ShopManager.Instance.machines[4].isPurchased = value;
            UpdateAllUI();
        }
    }
    public bool breaker
    {
        get => ShopManager.Instance.machines[0].isPurchased;
        set
        {
            ShopManager.Instance.machines[0].isPurchased = value;
            UpdateAllUI();
        }
    }
    public bool compressor
    {
        get => ShopManager.Instance.machines[3].isPurchased;
        set
        {
            ShopManager.Instance.machines[3].isPurchased = value;
            UpdateAllUI();
        }
    }

    private GameObject[] _m_block;
    private GameObject[] _b_block;
    private GameObject[] _c_block;
    private GameObject[] _mb_block;
    private GameObject[] _mc_block;
    private GameObject[] _bc_block;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedUIUpdate());
    }

    IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForEndOfFrame();
        CacheUIElements();
        UpdateAllUI();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
        UpdateAllUI();
#endif
    }

    private GameObject[] FindObjectsWithTagIncludingInactive(string tag)
    {
        List<GameObject> result = new List<GameObject>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.scene.IsValid())
            {
                result.Add(obj);
            }
        }
        return result.ToArray();
    }

    private void CacheUIElements()
    {
        _m_block = FindObjectsWithTagIncludingInactive("m_block");
        _b_block = FindObjectsWithTagIncludingInactive("b_block");
        _c_block = FindObjectsWithTagIncludingInactive("c_block");
        _mb_block = FindObjectsWithTagIncludingInactive("mb_block");
        _mc_block = FindObjectsWithTagIncludingInactive("mc_block");
        _bc_block = FindObjectsWithTagIncludingInactive("bc_block");
    }

    private void UpdateAllUI()
    {

        if (_m_block == null || _m_block.Length == 0)
        {
            return;
        }

        if (ShopManager.Instance == null || ShopManager.Instance.machines.Count < 5)
            return;

        // 각 기계의 구매 상태 직접 참조
        bool machinePurchased = ShopManager.Instance.machines[2].isPurchased;
        bool breakerPurchased = ShopManager.Instance.machines[0].isPurchased;
        bool blastFurnacePurchased = ShopManager.Instance.machines[4].isPurchased;
        bool compressorPurchased = ShopManager.Instance.machines[3].isPurchased;

        bool MB = machinePurchased && blastFurnacePurchased && breakerPurchased;
        bool MC = machinePurchased && compressorPurchased;
        bool BC = blastFurnacePurchased && breakerPurchased && compressorPurchased;


        // UI 갱신
        SetUIState(_m_block, !machinePurchased, "m_block");
        SetUIState(_b_block, !blastFurnacePurchased, "b_block");
        SetUIState(_c_block, !compressorPurchased, "c_block");

        SetUIState(_mb_block, !MB, "mb_block");
        SetUIState(_mc_block, !MC, "mc_block");
        SetUIState(_bc_block, !BC, "bc_block");
    }

    private void SetUIState(GameObject[] uiArray, bool state, string logName)
    {
        if (uiArray == null)
        {
            return;
        }

        foreach (var ui in uiArray)
        {
            if (ui == null)
            {
                continue;
            }

            if (ui.activeSelf != state)
            {
                ui.SetActive(state);
            }
        }
    }

    [ContextMenu("UI 요소 재탐색")]
    public void RefreshUI()
    {
        CacheUIElements();
        UpdateAllUI();
    }

    [ContextMenu("머신 상태 토글")]
    public void ToggleMachine() => machine = !machine;

    [ContextMenu("강제 UI 갱신")]
    public void ForceUIUpdate()
    {
        CacheUIElements();
        UpdateAllUI();
    }
}
