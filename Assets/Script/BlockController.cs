using UnityEngine;

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
            }
        }
    }

    private GameObject[] _m_block;
    private GameObject[] _b_block;
    private GameObject[] _c_block;
    private GameObject[] _mb_block;
    private GameObject[] _mc_block;
    private GameObject[] _bc_block;

    void Start()
    {
        _m_block = GameObject.FindGameObjectsWithTag("m_block");
        _b_block = GameObject.FindGameObjectsWithTag("b_block");
        _c_block = GameObject.FindGameObjectsWithTag("c_block");
        _mb_block = GameObject.FindGameObjectsWithTag("mb_block");
        _mc_block = GameObject.FindGameObjectsWithTag("mc_block");
        _bc_block = GameObject.FindGameObjectsWithTag("bc_block");
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        if (_m_block == null) return;

        bool MB = _machine && _blastFurnace && _breaker;
        bool MC = _machine && _compressor;
        bool BC = _blastFurnace && _breaker && _compressor;

        // m_block: machine이 켜지면 비활성화
        foreach (var ui in _m_block)
        {
            if (ui != null)
            {
                ui.SetActive(!_machine);
            }
        }

        // b_block: blastFurnace가 켜지면 비활성화
        foreach (var ui in _b_block)
        {
            if (ui != null)
            {
                ui.SetActive(!_blastFurnace);
            }
        }

        // c_block: compressor가 켜지면 비활성화
        foreach (var ui in _c_block)
        {
            if (ui != null)
            {
                ui.SetActive(!_compressor);
            }
        }

        // mb_block: machine과 blastFurnace가 모두 켜져야 활성화
        foreach (var ui in _mb_block)
        {
            if (ui != null)
            {
                ui.SetActive(!MB);
            }
        }

        // mc_block: machine과 compressor가 모두 켜져야 활성화
        foreach (var ui in _mc_block)
        {
            if (ui != null)
            {
                ui.SetActive(!MC);
            }
        }

        // bc_block: blastFurnace와 compressor가 모두 켜져야 활성화
        foreach (var ui in _bc_block)
        {
            if (ui != null)
            {
                ui.SetActive(!BC);
            }
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateAllUI();
        }
    }

    [ContextMenu("머신 상태 토글")]
    public void ToggleMachine()
    {
        machine = !machine;
    }
}
