using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    public Transform playerHand; // 플레이어 손
    public LayerMask floorLayer, wallLayer, ceilingLayer;
    public float placeDistance = 0.5f;
    public List<PlaceableItem> prefab;

    private GameObject heldItem;
    private GameObject previewItem;  // 미리보기 아이템
    private bool isPreviewActive = false;  // 미리보기 아이템 활성화 여부

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

        // 미리보기 아이템이 활성화되어 있으면 위치 업데이트
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
        heldItem.SetActive(false);  // 원본 heldItem은 숨겨진 채 존재

        // 미리보기 아이템을 생성하고 활성화
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

        previewItem.transform.rotation = placeRot;  // 미리보기 아이템에 즉시 회전 적용
        Debug.DrawRay(playerHand.position, playerHand.forward * placeDistance, Color.blue, 0.1f); // 레이 시각화 (색상 조절)

        // 배치할 위치를 결정하는 로직 (미리보기 아이템 위치 설정)
        switch (item.placeType)
        {
            case PlaceType.Floor:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, floorLayer))
                {
                    if (item.name == "Old Chest")
                    {
                        placePos = hit.point + new Vector3(0, 0.5f, 0); // 바닥에서 조금 더 위로 배치
                    }

                    else
                    {
                        placePos = hit.point + new Vector3(0, 0, 0); // 바닥에서 조금 더 위로 배치
                    }

                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; // 최소 거리 0.5로 설정
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
                    // 플레이어로부터 벽까지의 거리가 너무 가까우면 배치하지 않음
                    float minDistanceToWall = 0.6f; // 이 값은 벽에 얼마나 가까이 붙을 수 있는지 결정 (조절 가능)
                    if (hit.distance < minDistanceToWall)
                    {
                        // 거리가 너무 가까우면 미리보기 아이템을 비활성화하고 함수 종료
                        if (previewItem != null)
                        {
                            previewItem.SetActive(false);
                        }
                        return; // 더 이상 진행하지 않고 함수 종료
                    }

                    Vector3 wallNormal = hit.normal;  // 벽의 법선 벡터 (벽면으로부터 바깥쪽을 향하는 방향)
                                                      // hit.point에서 벽의 법선 방향으로 0.07f (아이템이 벽에서 띄워질 거리) 떨어진 위치
                    placePos = hit.point + wallNormal * 0.07f;

                    // 시계의 '앞면'(파란색 Z축)이 벽의 법선 반대 방향(-wallNormal, 즉 플레이어 쪽)을 바라보도록 회전
                    // '위쪽'(초록색 Y축)은 월드 공간의 위쪽(Vector3.up)을 향하도록 설정
                    placeRot = Quaternion.LookRotation(-wallNormal, Vector3.up);

                    // 시계 모델의 기본 방향이 뒤집혀 있는 경우 (유니티 모델의 기본 Z축 방향에 따라 다름)
                    // Y축으로 180도 추가 회전하여 시계면이 플레이어를 정확히 바라보게 함
                    placeRot *= Quaternion.Euler(0, 180, 0);

                    // 벽에 레이가 성공적으로 닿았고, 거리가 적절하므로 미리보기 활성화 및 위치/회전 즉시 적용
                    if (previewItem != null)
                    {
                        previewItem.SetActive(true); // 미리보기 활성화
                        previewItem.transform.position = placePos; // 계산된 벽 위치로 설정
                        previewItem.transform.rotation = placeRot; // 계산된 벽 회전으로 설정
                    }
                }
                else // 벽에 레이가 쏘이지 않으면 미리보기 아이템을 비활성화
                {
                    if (previewItem != null)
                    {
                        previewItem.SetActive(false);
                    }
                    return; // 더 이상 진행하지 않고 함수 종료
                }
                break; // Wall 케이스 종료

            case PlaceType.Ceiling:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, ceilingLayer))
                {
                    placePos = hit.point + new Vector3(0, -1.8f, 0);  // 천장에서 조금 더 위로
                                                                      // 플레이어와 너무 가까운지 체크하여 최소 거리 설정
                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; // 최소 거리 0.5로 설정
                    }
                }
                else
                    return;
                break;

            default:
                return;
        }

        // 미리보기 아이템을 설정된 위치와 회전으로 이동
        previewItem.transform.position = placePos;
        previewItem.transform.rotation = placeRot;

        // 미리보기 아이템의 콜라이더를 비활성화하여 플레이어와 충돌하지 않게 함
        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;  // 콜라이더 비활성화
        }
    }

    void TryPlaceItem()
    {
        if (previewItem == null) return; // 미리보기 아이템이 없으면 배치 시도 안 함

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return; // 들고 있는 아이템이 PlaceableItem이 아니면 배치 시도 안 함

        Vector3 finalPlacePos = Vector3.zero; // 최종 배치될 위치
        Quaternion finalPlaceRot = Quaternion.identity; // 최종 배치될 회전

        // 미리보기 아이템이 활성화되어 있지 않으면 (즉, 유효한 배치 위치가 없으면) 배치하지 않음
        if (!previewItem.activeSelf)
        {
            Debug.Log("미리보기 아이템이 비활성화 상태이므로 배치할 수 없습니다. 유효한 위치를 찾아주세요.");
            return; // 배치할 수 없으므로 함수 종료
        }

        // 아이템 타입에 따라 최종 배치 위치와 회전을 결정
        switch (item.placeType)
        {
            case PlaceType.Wall: // 벽 아이템인 경우
                RaycastHit hit;
                // 배치 시점에 다시 한번 레이캐스트를 쏴서 현재 유효한 벽 위치를 찾음
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, wallLayer))
                {
                    // 벽에 너무 가까운지 다시 한번 검사 (UpdatePreviewItem과 동일)
                    float minDistanceToWall = 0.6f;
                    if (hit.distance < minDistanceToWall)
                    {
                        Debug.Log("벽에 너무 가까워서 배치할 수 없습니다.");
                        return; // 너무 가까우면 배치하지 않고 함수 종료
                    }

                    Vector3 wallNormal = hit.normal;
                    finalPlacePos = hit.point + wallNormal * 0.07f; // 벽에서 약간 떨어진 위치
                    finalPlaceRot = Quaternion.LookRotation(-wallNormal, Vector3.up); // 벽 법선에 맞춰 회전
                    finalPlaceRot *= Quaternion.Euler(0, 180, 0); // 180도 추가 회전
                }
                else // 벽에 레이가 닿지 않으면 배치 불가능
                {
                    Debug.Log("벽이 감지되지 않아 벽 아이템을 배치할 수 없습니다.");
                    return; // 벽이 없으면 배치하지 않고 함수 종료
                }
                break;

            case PlaceType.Floor: // 바닥 아이템인 경우 (미리보기 위치/회전 사용)
            case PlaceType.Ceiling: // 천장 아이템인 경우 (미리보기 위치/회전 사용)
                                    // 미리보기 아이템의 위치와 회전을 그대로 사용합니다.
                                    // UpdatePreviewItem에서 이미 정확히 계산되었을 것이라고 가정합니다.
                finalPlacePos = previewItem.transform.position;
                finalPlaceRot = previewItem.transform.rotation;
                break;

            default:
                Debug.LogWarning($"알 수 없는 PlaceType: {item.placeType}. 배치할 수 없습니다.");
                return;
        }

        // 모든 검사를 통과하고 최종 위치/회전이 결정되면 아이템을 배치
        GameObject placedObject = Instantiate(heldItem, finalPlacePos, finalPlaceRot);
        placedObject.SetActive(true); // 배치된 아이템 활성화

        // 배치 후에 콜라이더를 다시 활성화
        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = true; // 배치된 아이템의 콜라이더 활성화
        }

        // heldItem을 삭제하고 손에 아이템을 들고 있지 않게 처리
        Destroy(heldItem); // 원본 heldItem은 Destroy
        heldItem = null;

        // 미리보기 아이템 삭제
        Destroy(previewItem);
        previewItem = null; // 미리보기 아이템 초기화
        isPreviewActive = false; // 미리보기 비활성화

        // 아이템 배치 정보 업데이트
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

    // 미리보기 아이템의 투명도를 설정하는 함수
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
                    // 현재 사용하는 쉐이더가 투명도를 지원하는지 확인
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
