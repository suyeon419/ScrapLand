using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPortal : MonoBehaviour
{
    public Transform homePos;
    public GameObject player;
    public GameObject Camera;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
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
