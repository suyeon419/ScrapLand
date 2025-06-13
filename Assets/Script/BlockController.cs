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
                Debug.Log($"�ӽ� ���� ����: {_machine}");
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
                Debug.Log($"�뱤�� ���� ����: {_blastFurnace}");
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
                Debug.Log($"�극��Ŀ ���� ����: {_breaker}");
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
                Debug.Log($"����� ���� ����: {_compressor}");
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
        Debug.Log($"�� �ε� �Ϸ�: {scene.name}");
        StartCoroutine(DelayedUIUpdate());
    }

    IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("### UI ���� ���� ���� ###");
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
        Debug.Log($"{tag} ã�� ����: {result.Count}");
        return result.ToArray();
    }

    private void CacheUIElements()
    {
        Debug.Log("UI ��� Ž�� ���� ==================");
        _m_block = FindObjectsWithTagIncludingInactive("m_block");
        _b_block = FindObjectsWithTagIncludingInactive("b_block");
        _c_block = FindObjectsWithTagIncludingInactive("c_block");
        _mb_block = FindObjectsWithTagIncludingInactive("mb_block");
        _mc_block = FindObjectsWithTagIncludingInactive("mc_block");
        _bc_block = FindObjectsWithTagIncludingInactive("bc_block");
        Debug.Log("UI ��� Ž�� ���� ==================");
    }

    private void UpdateAllUI()
    {
        Debug.Log($"UI ���� ȣ�� | �ӽ�:{_machine} �뱤��:{_blastFurnace} �극��Ŀ:{_breaker} �����:{_compressor}");

        if (_m_block == null || _m_block.Length == 0)
        {
            Debug.LogError("m_block ��Ҹ� ã�� �� �����ϴ�!");
            return;
        }

        // ���ǽ��� �ǵ���� ����ϼ���.
        bool MB = _machine && _blastFurnace && _breaker;
        bool MC = _machine && _compressor;
        bool BC = _blastFurnace && _breaker && _compressor;

        Debug.Log($"���� ��� -> MB:{MB} MC:{MC} BC:{BC}");

        Debug.Log("���� ��� ó�� ����");
        SetUIState(_m_block, !_machine, "m_block");
        SetUIState(_b_block, !_blastFurnace, "b_block");
        SetUIState(_c_block, !_compressor, "c_block");

        Debug.Log("���� ��� ó�� ����");
        SetUIState(_mb_block, !MB, "mb_block");
        SetUIState(_mc_block, !MC, "mc_block");
        SetUIState(_bc_block, !BC, "bc_block");
    }

    private void SetUIState(GameObject[] uiArray, bool state, string logName)
    {
        if (uiArray == null)
        {
            Debug.LogError($"{logName} �迭�� null�Դϴ�!");
            return;
        }

        foreach (var ui in uiArray)
        {
            if (ui == null)
            {
                Debug.LogWarning($"{logName} �迭�� null ������Ʈ ����!");
                continue;
            }

            if (ui.activeSelf != state)
            {
                Debug.Log($"{logName} [{ui.name}] ���� ����: {ui.activeSelf} -> {state}");
                ui.SetActive(state);
            }
            else
            {
                Debug.Log($"{logName} [{ui.name}] ���� ����: {state}");
            }
        }
    }

    [ContextMenu("UI ��� ��Ž��")]
    public void RefreshUI()
    {
        CacheUIElements();
        UpdateAllUI();
    }

    [ContextMenu("�ӽ� ���� ���")]
    public void ToggleMachine() => machine = !machine;

    [ContextMenu("���� UI ����")]
    public void ForceUIUpdate()
    {
        CacheUIElements();
        UpdateAllUI();
    }
}
