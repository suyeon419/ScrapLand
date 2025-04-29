using UnityEngine;

public class BlockController : MonoBehaviour
{
    public bool IsBlocked = true;
    public GameObject Block;

    void Update()
    {
        if (Block != null)
        {
            Block.SetActive(IsBlocked);
        }
    }
}
