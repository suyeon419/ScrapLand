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
                Debug.Log($"머신 상태 변경: {_machine}");
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
                Debug.Log($"용광로 상태 변경: {_blastFurnace}");
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
                Debug.Log($"브레이커 상태 변경: {_breaker}");
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
                Debug.Log($"압축기 상태 변경: {_compressor}");
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
        Debug.Log($"씬 로드 완료: {scene.name}");
        StartCoroutine(DelayedUIUpdate());
    }

    IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("### UI 지연 갱신 시작 ###");
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
        Debug.Log($"{tag} 찾은 개수: {result.Count}");
        return result.ToArray();
    }

    private void CacheUIElements()
    {
        Debug.Log("UI 요소 탐색 시작 ==================");
        _m_block = FindObjectsWithTagIncludingInactive("m_block");
        _b_block = FindObjectsWithTagIncludingInactive("b_block");
        _c_block = FindObjectsWithTagIncludingInactive("c_block");
        _mb_block = FindObjectsWithTagIncludingInactive("mb_block");
        _mc_block = FindObjectsWithTagIncludingInactive("mc_block");
        _bc_block = FindObjectsWithTagIncludingInactive("bc_block");
        Debug.Log("UI 요소 탐색 종료 ==================");
    }

    private void UpdateAllUI()
    {
        Debug.Log($"UI 갱신 호출 | 머신:{_machine} 용광로:{_blastFurnace} 브레이커:{_breaker} 압축기:{_compressor}");

        if (_m_block == null || _m_block.Length == 0)
        {
            Debug.LogError("m_block 요소를 찾을 수 없습니다!");
            return;
        }

        // 조건식은 의도대로 사용하세요.
        bool MB = _machine && _blastFurnace && _breaker;
        bool MC = _machine && _compressor;
        bool BC = _blastFurnace && _breaker && _compressor;

        Debug.Log($"조건 계산 -> MB:{MB} MC:{MC} BC:{BC}");

        Debug.Log("단일 블록 처리 시작");
        SetUIState(_m_block, !_machine, "m_block");
        SetUIState(_b_block, !_blastFurnace, "b_block");
        SetUIState(_c_block, !_compressor, "c_block");

        Debug.Log("조합 블록 처리 시작");
        SetUIState(_mb_block, !MB, "mb_block");
        SetUIState(_mc_block, !MC, "mc_block");
        SetUIState(_bc_block, !BC, "bc_block");
    }

    private void SetUIState(GameObject[] uiArray, bool state, string logName)
    {
        if (uiArray == null)
        {
            Debug.LogError($"{logName} 배열이 null입니다!");
            return;
        }

        foreach (var ui in uiArray)
        {
            if (ui == null)
            {
                Debug.LogWarning($"{logName} 배열에 null 오브젝트 포함!");
                continue;
            }

            if (ui.activeSelf != state)
            {
                Debug.Log($"{logName} [{ui.name}] 상태 변경: {ui.activeSelf} -> {state}");
                ui.SetActive(state);
            }
            else
            {
                Debug.Log($"{logName} [{ui.name}] 상태 유지: {state}");
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
