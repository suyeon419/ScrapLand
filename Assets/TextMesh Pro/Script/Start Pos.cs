using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    public Transform startPos;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        if (startPos == null)
        {
            GameObject obj = GameObject.Find("Map Portal pos");
            if (obj != null)
                startPos = obj.transform;
            else
                Debug.LogWarning("Map Portal pos를 찾을 수 없습니다.");
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
                Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
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
