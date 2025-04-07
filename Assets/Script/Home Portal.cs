using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePortal : MonoBehaviour
{
    public Transform mapPos;
    public GameObject player;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {

            Debug.Log("캐릭터 이동");
            CharacterController controller = player.GetComponent<CharacterController>();

            Vector3 pos = mapPos.position;

            controller.enabled = false;
            player.transform.position = pos;
            controller.enabled = true;
            Debug.Log("캐릭터 이동 완료");
        }
    }
}
