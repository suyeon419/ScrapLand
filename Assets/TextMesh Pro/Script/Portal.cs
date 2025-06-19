using System.Collections;
using UnityEngine;
using Controller; 

public class Portal : MonoBehaviour
{
    [Header("���� ����")]
    public Transform destination; 
    public GameObject player;

    [Header("ī�޶� ����")]
    public ThirdPersonCamera thirdPersonCamera;

    [Header("ī�޶� �� ����")]
    [Tooltip("���� ��� �� ����� Zoom �� (0~1)")]
    [Range(0f, 1f)]
    public float targetZoom = 0.5f; 
    public float zoomDuration = 0.5f; 

    void Awake()
    {
        if (thirdPersonCamera == null && Camera.main != null)
        {
            thirdPersonCamera = Camera.main.GetComponent<ThirdPersonCamera>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TeleportPlayer();

            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.AnimateZoom(targetZoom, zoomDuration);
            }
            else
            {
                Debug.LogError("ThirdPersonCamera ��ũ��Ʈ�� �Ҵ� ��");
            }
        }
    }

    private void TeleportPlayer()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (destination.name.Contains("Home Portal"))
        {
            PlacementManager.Instance.isHome = true;
            //string item = InventorySelectionManager.Instance.GetSelectedItemType();
            //PlacementManager.Instance.SetHeldItem(item);
        }
        else
        {
            PlacementManager.Instance.isHome = false;
        }
            
        controller.enabled = false;
        player.transform.position = destination.position;
        if (Camera.main != null)
        {
            Camera.main.transform.position = player.transform.position;
        }
        controller.enabled = true;
    }
}