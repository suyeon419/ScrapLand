using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePortal : MonoBehaviour
{
    public Transform mapPos;
    public GameObject player;
    public GameObject Camera;

    void Awake()
    {
        if (mapPos == null)
        {
            GameObject obj = GameObject.Find("Map Portal pos");
            if (obj != null)
                mapPos = obj.transform;
            else
                Debug.LogWarning("Map Portal pos를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController controller = player.GetComponent<CharacterController>();

            Vector3 pos = mapPos.position;

            controller.enabled = false;
            player.transform.position = pos;
            controller.enabled = true;
            Camera.transform.position = pos;
        }
    }
}