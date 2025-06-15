using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class MapPortal : MonoBehaviour
{
    public Transform homePos;
    public GameObject player;
    public GameObject Camera;

    void Awake()
    {
        if (homePos == null)
        {
            GameObject obj = GameObject.Find("Home Portal pos");
            if (obj != null)
                homePos = obj.transform;
            else
                Debug.LogWarning("Map Portal pos를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController controller = player.GetComponent<CharacterController>();

            Vector3 pos = homePos.position;

            controller.enabled = false;
            player.transform.position = pos;
            controller.enabled = true;
            Camera.transform.position = pos;
        }
    }
}