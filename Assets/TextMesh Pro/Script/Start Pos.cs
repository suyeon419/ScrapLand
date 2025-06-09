using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    public Transform startPos;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition();
    }

    public void StartPosition()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        Vector3 pos = startPos.position;

        controller.enabled = false;
        player.transform.position = pos;
        controller.enabled = true;  
    }
}
