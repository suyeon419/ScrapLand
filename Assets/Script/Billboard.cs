using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.forward = -transform.forward;
    }
}
