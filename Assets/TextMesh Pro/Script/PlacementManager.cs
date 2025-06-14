using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    public Transform playerHand; 
    public LayerMask floorLayer, wallLayer, ceilingLayer;
    public float placeDistance = 0.5f;
    public List<PlaceableItem> prefab;

    private GameObject heldItem;
    private GameObject previewItem;  
    private bool isPreviewActive = false; 

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
    }

    void Update()
    {
        if (heldItem == null) return;

        HandleRotation();

        if (isPreviewActive)
        {
            UpdatePreviewItem();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            TryPlaceItem();
        }
    }

    public void SetHeldItem(string itemName)
    {
        if (!itemPrefabs.ContainsKey(itemName))
        {
            Debug.Log($"'{itemName}' 프리팹이 존재하지 않습니다.");
            return;
        }

        GameObject prefab = itemPrefabs[itemName];
        heldItem = Instantiate(prefab);
        heldItem.SetActive(false);  

        if (previewItem == null)
        {
            previewItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            previewItem.SetActive(true);
            isPreviewActive = true;
            SetPreviewItemTransparency(0.3f);
        }
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
        if (previewItem == null) return;

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        Vector3 placePos;
        Quaternion placeRot = heldItem.transform.rotation;
        RaycastHit hit;

        previewItem.transform.rotation = placeRot;  

        switch (item.placeType)
        {
            case PlaceType.Floor:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, floorLayer))
                {
                    if (item.name == "Old Chest")
                    {
                        placePos = hit.point + new Vector3(0, 0.5f, 0); 
                    }

                    else
                    {
                        placePos = hit.point + new Vector3(0, 0, 0); 
                    }

                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; 
                    }
                }
                else
                {
                    return;
                }
                break;

            case PlaceType.Wall:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, wallLayer))
                {
                    float minDistanceToWall = 0.6f; 
                    if (hit.distance < minDistanceToWall)
                    {
                        if (previewItem != null)
                        {
                            previewItem.SetActive(false);
                        }
                        return; 
                    }

                    Vector3 wallNormal = hit.normal;  
                                                      
                    placePos = hit.point + wallNormal * 0.07f;

                    placeRot = Quaternion.LookRotation(-wallNormal, Vector3.up);

                    placeRot *= Quaternion.Euler(0, 180, 0);

                    if (previewItem != null)
                    {
                        previewItem.SetActive(true); 
                        previewItem.transform.position = placePos; 
                        previewItem.transform.rotation = placeRot; 
                    }
                }
                else 
                {
                    if (previewItem != null)
                    {
                        previewItem.SetActive(false);
                    }
                    return; 
                }
                break; 

            case PlaceType.Ceiling:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, ceilingLayer))
                {
                    placePos = hit.point + new Vector3(0, -1.8f, 0); 
                                                                      
                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; 
                    }
                }
                else
                    return;
                break;

            default:
                return;
        }

        previewItem.transform.position = placePos;
        previewItem.transform.rotation = placeRot;

        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;  // 콜라이더 비활성화
        }
    }

    void TryPlaceItem()
    {
        if (previewItem == null) return; 

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return; 

        Vector3 finalPlacePos = Vector3.zero; 
        Quaternion finalPlaceRot = Quaternion.identity; 

        if (!previewItem.activeSelf)
        {
            return;
        }

        switch (item.placeType)
        {
            case PlaceType.Wall: 
                RaycastHit hit;
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, wallLayer))
                {
                    float minDistanceToWall = 0.6f;
                    if (hit.distance < minDistanceToWall)
                    {
                        Debug.Log("벽에 너무 가까워서 배치할 수 없습니다.");
                        return; 
                    }

                    Vector3 wallNormal = hit.normal;
                    finalPlacePos = hit.point + wallNormal * 0.07f;
                    finalPlaceRot = Quaternion.LookRotation(-wallNormal, Vector3.up); 
                    finalPlaceRot *= Quaternion.Euler(0, 180, 0); 
                }
                else
                {
                    return; 
                }
                break;

            case PlaceType.Floor: 
            case PlaceType.Ceiling: 
                                    
                finalPlacePos = previewItem.transform.position;
                finalPlaceRot = previewItem.transform.rotation;
                break;

            default:
                return;
        }

        GameObject placedObject = Instantiate(heldItem, finalPlacePos, finalPlaceRot);
        placedObject.SetActive(true); 

        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = true; 
        }

        Destroy(heldItem); 
        heldItem = null;

        Destroy(previewItem);
        previewItem = null; 
        isPreviewActive = false; 

        UpdatePlacementInfo(item.itemName, finalPlacePos, finalPlaceRot);
    }


    void UpdatePlacementInfo(string itemName, Vector3 placePos, Quaternion placeRot)
    {
        if (itemScores.ContainsKey(itemName))
        {
            Vector3 rotationEuler = placeRot.eulerAngles;
            HappyEarth.instance.Install_Interior(itemName, itemScores[itemName], placePos, rotationEuler);
            int score = itemScores[itemName];
            Debug.Log($"{itemName} 배치됨. 점수: {score}");
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
                        color = new Color(0f / 255f, 255f / 255f, 255f / 255f, alpha);
                        material.color = color;

                        material.SetFloat("_Mode", 3); // Transparent 모드로 설정
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
