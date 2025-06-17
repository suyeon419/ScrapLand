using System.Collections;
using UnityEngine;
using Controller; 

public class Portal : MonoBehaviour
{
    [Header("포털 설정")]
    public Transform destination; 
    public GameObject player;

    [Header("카메라 설정")]
    public ThirdPersonCamera thirdPersonCamera;

    [Header("카메라 줌 설정")]
    [Tooltip("포털 통과 시 변경될 Zoom 값 (0~1)")]
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
                Debug.LogError("ThirdPersonCamera 스크립트가 할당 않");
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