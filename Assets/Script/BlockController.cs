using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlockController : MonoBehaviour
{
    [SerializeField] private bool _machine;
    [SerializeField] private bool _breaker;
    [SerializeField] private bool _blastFurnace;
    [SerializeField] private bool _compressor;

    public bool machine
    {
        get => _machine;
        set
        {
            if (_machine != value)
            {
                _machine = value;
                UpdateAllUI();
            }
        }
    }

    public bool blastFurnace
    {
        get => _blastFurnace;
        set
        {
            if (_blastFurnace != value)
            {
                _blastFurnace = value;
                UpdateAllUI();
            }
        }
    }
    public bool breaker
    {
        get => _breaker;
        set
        {
            if (_breaker != value)
            {
                _breaker = value;
                UpdateAllUI();
            }
        }
    }
    public bool compressor
    {
        get => _compressor;
        set
        {
            if (_compressor != value)
            {
                _compressor = value;
                UpdateAllUI();
            }
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

        // 조건식은 의도대로 사용하세요.
        bool MB = _machine && _blastFurnace && _breaker;
        bool MC = _machine && _compressor;
        bool BC = _blastFurnace && _breaker && _compressor;


        SetUIState(_m_block, !_machine, "m_block");
        SetUIState(_b_block, !_blastFurnace, "b_block");
        SetUIState(_c_block, !_compressor, "c_block");

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
