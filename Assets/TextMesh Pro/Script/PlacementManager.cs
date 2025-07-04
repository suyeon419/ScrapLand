using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class PlacementManager : MonoBehaviour
{
    public LayerMask floorLayer, wallLayer, ceilingLayer;
    public float placeDistance = 50f; 
    public List<PlaceableItem> prefab;
    public Transform playerHand;
    
    [HideInInspector]
    public bool isHome = false;


    private GameObject heldItem;
    private GameObject previewItem;
    private bool isPreviewActive = false;
    private bool isOverlapping = false;
    private bool isChest = false;

    private Camera mainCamera; 

    private static PlacementManager instance = null;
    public static PlacementManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private Dictionary<string, int> itemScores = new Dictionary<string, int>()
    {
        { "Bench", 45 },
        { "Can Pot", 28 },
        { "Clock", 50 },
        { "Glass Pot", 42 },
        { "Mobile", 48 },
        { "Old Chest", 43 },
        { "Plastic Pot", 28 },
        { "Table", 52 }
    };


    Dictionary<string, GameObject> itemPrefabs = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (var entry in prefab)
            {
                if (!itemPrefabs.ContainsKey(entry.itemName))
                {
                    itemPrefabs.Add(entry.itemName, entry.prefab);
                }
                else
                {
                    Debug.LogWarning($"Awake: '{entry.itemName}' 프리팹이 이미 itemPrefabs에 존재합니다.");
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (playerHand == null)
        {
            GameObject obj = GameObject.Find("Head");
            if (obj != null)
                playerHand = obj.transform;
            else
                Debug.LogWarning("Map Portal pos를 찾을 수 없습니다.");
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("메인 카메라 (태그: MainCamera)를 찾을 수 없습니다. 씬에 메인 카메라가 있고 'MainCamera' 태그가 지정되어 있는지 확인하세요.");
        }
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (heldItem == null) return;
        if (mainCamera == null)
        {
            Debug.LogError("메인 카메라가 없습니다. PlacementManager가 작동하지 않습니다.");
            return;
        }

        HandleRotation();

        if (isPreviewActive)
        {
            if (isHome && !isChest)
            {
                UpdatePreviewItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isHome && !isChest)
            {
                TryPlaceItem();
                if (!isOverlapping)
                {
                    InventorySelectionManager.Instance.RemoveSelectedHotBarItem();
                }
            }

            else 
            {
                return;
            }
        }
    }

    public void SetHeldItem(string itemName)
    {
        if (!itemPrefabs.ContainsKey(itemName))
        {
            if (heldItem != null)
            {
                Destroy(heldItem);
                heldItem = null; 
            }
            if (previewItem != null)
            {
                Destroy(previewItem);
                previewItem = null; 
            }
            isPreviewActive = false; 

            return; 
        }

        if (!isHome)
        {
            return;
        }

        GameObject prefabToInstantiate = itemPrefabs[itemName];

        if (heldItem != null)
        {
            Destroy(heldItem);
            heldItem = null;
        }
        if (previewItem != null)
        {
            Destroy(previewItem);
            previewItem = null;
        }
        isPreviewActive = false;

        heldItem = Instantiate(prefabToInstantiate);
        heldItem.SetActive(false);
        Debug.Log($"SetHeldItem: heldItem '{itemName}' 생성 및 비활성화됨.");

        previewItem = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity);
        previewItem.SetActive(true);
        isPreviewActive = true;
        SetPreviewItemTransparency(0.3f);
        Debug.Log($"SetHeldItem: previewItem '{itemName}' 생성 및 활성화됨.");
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            heldItem.transform.Rotate(Vector3.up, -90f);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            heldItem.transform.Rotate(Vector3.up, 90f);
    }

    void UpdatePreviewItem()
    {
        if (previewItem == null || mainCamera == null) return; // thirdPersonCam 대신 mainCamera 사용

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        Vector3 placePos;
        Quaternion placeRot = heldItem.transform.rotation;
        RaycastHit hit;

        previewItem.transform.rotation = placeRot;

        Vector3 rayOrigin = mainCamera.transform.position;
        Vector3 rayDirection = mainCamera.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * placeDistance, Color.yellow); // 미리보기 레이 시각화

        switch (item.placeType)
        {
            case PlaceType.Floor:
                if (Physics.Raycast(rayOrigin, rayDirection, out hit, placeDistance, floorLayer))
                {
                    if (item.name.Contains("Old Chest")|| item.name.Contains("breaker") || item.name.Contains("sewing")) 
                    {
                        placePos = hit.point + new Vector3(0, 0.5f, 0);
                    }
                    else
                    {
                        placePos = hit.point; 
                    }

                    if (Vector3.Distance(placePos, rayOrigin) < 0.5f)
                    {
                        placePos = rayOrigin + rayDirection * 0.5f;
                    }

                    if (!previewItem.activeSelf) previewItem.SetActive(true);
                    previewItem.transform.position = placePos;
                    previewItem.transform.rotation = placeRot;
                }
                else
                {
                    if (previewItem.activeSelf) previewItem.SetActive(false); 
                    return;
                }
                break;

            case PlaceType.Wall:
                if (Physics.Raycast(rayOrigin, rayDirection, out hit, placeDistance, wallLayer))
                {
                    float minDistanceToWall = 0.6f;
                    if (hit.distance < minDistanceToWall)
                    {
                        if (previewItem.activeSelf) previewItem.SetActive(false);
                        return;
                    }

                    Vector3 wallNormal = hit.normal;
                    placePos = hit.point + wallNormal * 0.07f;

                    placeRot = Quaternion.LookRotation(-wallNormal, Vector3.up); 

                    if (!previewItem.activeSelf) previewItem.SetActive(true);
                    previewItem.transform.position = placePos;
                    previewItem.transform.rotation = placeRot;
                }
                else
                {
                    if (previewItem.activeSelf) previewItem.SetActive(false);
                    return;
                }
                break;

            case PlaceType.Ceiling:
                if (playerHand == null)
                {
                    Debug.LogWarning("playerHand가 할당되지 않아 천장 아이템 미리보기가 불가능합니다.");
                    return;
                }

                Debug.DrawRay(playerHand.position, Vector3.up * placeDistance, Color.cyan);

                if (Physics.Raycast(playerHand.position, Vector3.up, out hit, placeDistance, ceilingLayer))
                {
                    float forwardOffset = 0.5f;
                    placePos = hit.point + new Vector3(0, -1.8f, 0); 
                    placePos += playerHand.forward * forwardOffset;

                    previewItem.transform.position = placePos;
                    previewItem.transform.rotation = placeRot;
                }
                break;

            default:
                if (previewItem.activeSelf) previewItem.SetActive(false); 
                return;
        }

        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false; 
        }
    }

    void TryPlaceItem()
    {
        if (previewItem == null) return;

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        if (!isPreviewActive || !previewItem.activeSelf) // isPreviewActive도 확인하여 미리보기가 활성화되지 않은 상태인지 확인
        {
            Debug.Log("미리보기 위치가 유효하지 않아 아이템을 배치할 수 없습니다.");
            return;
        }

        Vector3 finalPlacePos = previewItem.transform.position;
        Quaternion finalPlaceRot = previewItem.transform.rotation;

        Collider previewCollider = previewItem.GetComponent<Collider>();

        if (previewCollider != null)
        {
            // 겹침 검사를 위해 미리보기 콜라이더를 잠시 활성화
            previewCollider.enabled = true;

            // OverlapBox의 중심점 계산 개선
            // World space center of the collider, considering its current position and rotation
            Vector3 colliderWorldCenter = previewItem.transform.TransformPoint(previewCollider.bounds.center - previewItem.transform.position);
            Vector3 colliderHalfExtents = previewCollider.bounds.extents;

            Collider[] hitColliders = Physics.OverlapBox(colliderWorldCenter, colliderHalfExtents, finalPlaceRot);

            isOverlapping = false;
            foreach (Collider col in hitColliders)
            {
                // 자신(previewItem)이 아니고, 트리거가 아닌 다른 오브젝트와 겹치는지 확인
                if (col.gameObject != previewItem && col.isTrigger == false)
                {
                    // 특정 태그를 가진 오브젝트와 겹치는지 확인 (여기서 "previewItem" 자체의 콜라이더는 제외되어야 합니다)
                    if (col.CompareTag("Interior") || col.CompareTag("BlastFurnace") || col.CompareTag("breaker") || col.CompareTag("compressor") || col.CompareTag("machine") || col.CompareTag("sewing"))
                    {
                        isOverlapping = true;
                        break;
                    }
                }
            }

            // 겹침 검사 후 콜라이더를 다시 비활성화
            previewCollider.enabled = false;

            if (isOverlapping)
            {
                Debug.Log("다른 아이템과 겹쳐서 배치할 수 없습니다.");
                return;
            }
        }

        // 겹치지 않을 경우에만 아이템 배치 진행
        GameObject placedObject = Instantiate(heldItem, finalPlacePos, finalPlaceRot);
        placedObject.SetActive(true);

        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = true; // 배치된 아이템의 콜라이더는 활성화
        }

        // 기존 heldItem과 previewItem 정리
        Destroy(heldItem);
        heldItem = null;

        Destroy(previewItem);
        previewItem = null;
        isPreviewActive = false;

        // 추가 로직 (ShopManager, HappyEarth 등)
        if (item.tag == "BlastFurnace" || item.tag == "breaker" || item.tag == "compressor" || item.tag == "machine" || item.tag == "sewing")
        {
            if (itemPrefabs.ContainsKey(item.itemName))
            {
                Debug.Log(item.itemName);
                Vector3 rotationEuler = finalPlaceRot.eulerAngles;
                HappyEarth.instance.Install_Interior(item.itemName, 0, finalPlacePos, rotationEuler);
                ShopManager.Instance.GetOnMachine(item.itemName, true);
            }
        }

        if (item.itemName.Contains("Old Chest"))
        {
            isChest = true;
        }

        UpdatePlacementInfo(item.itemName, finalPlacePos, finalPlaceRot);
    }


    void UpdatePlacementInfo(string itemName, Vector3 placePos, Quaternion placeRot)
    {
        if (itemScores.ContainsKey(itemName))
        {
            Vector3 rotationEuler = placeRot.eulerAngles;
            if (HappyEarth.instance != null)
            {
                HappyEarth.instance.Install_Interior(itemName, itemScores[itemName], placePos, rotationEuler);
                int score = itemScores[itemName];
                Debug.Log($"{itemName} 배치됨. 점수: {score}");
            }
            else
            {
                Debug.LogWarning("HappyEarth.instance를 찾을 수 없습니다. 점수 업데이트가 불가능합니다.");
            }
        }
        else
        {
            Debug.Log($"{itemName} 배치됨. 점수 정보 없음.");
        }
    }

    void SetPreviewItemTransparency(float alpha)
    {
        Renderer[] renderers = previewItem.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                Material[] materials = renderer.materials;

                foreach (Material material in materials)
                {
                    if (material.HasProperty("_Color"))
                    {
                        Color color = material.color;
                        color = new Color(0f, 255f / 255f, 255f / 255f, alpha);
                        material.color = color;

                        material.SetFloat("_Mode", 3); 
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0); 
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = 3000; 
                    }
                }
            }
        }
    }

    public void ReLoadItem(string itemName, Vector3 placePos, Vector3 placeRot)
    {
        if (!itemPrefabs.ContainsKey(itemName))
        {
            Debug.Log($"'{itemName}' 프리팹이 존재하지 않습니다.");
            return;
        }
        Quaternion rotation = Quaternion.Euler(placeRot);

        GameObject prefab = itemPrefabs[itemName];

        GameObject install = Instantiate(prefab, placePos, rotation);
        install.SetActive(true);
    }
}