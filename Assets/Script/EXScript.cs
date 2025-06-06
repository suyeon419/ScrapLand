using UnityEngine;

public class EXScript : MonoBehaviour
{
    public BlockController blockController;

    [SerializeField] private bool _machine;
    [SerializeField] private bool _blastFurnace;
    [SerializeField] private bool _compressor;

    // 인스펙터에서 값이 바뀔 때마다 BlockController에 반영
    private void OnValidate()
    {
        if (blockController != null)
        {
            blockController.machine = _machine;
            blockController.blastFurnace = _blastFurnace;
            blockController.compressor = _compressor;
        }
    }
}
