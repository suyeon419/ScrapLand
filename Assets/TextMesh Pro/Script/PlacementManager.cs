using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    public Transform playerHand; // 플레이어 손
    public LayerMask floorLayer, wallLayer, ceilingLayer;
    public float placeDistance = 0.5f;

    private GameObject heldItem;
    private GameObject previewItem;  // 미리보기 아이템
    private bool isPreviewActive = false;  // 미리보기 아이템 활성화 여부
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

    public void SetHeldItem(GameObject item)
    {
        heldItem = item;

        // 미리보기 아이템을 생성하고 활성화
        if (previewItem == null)
        {
            previewItem = Instantiate(heldItem, Vector3.zero, Quaternion.identity);
            previewItem.SetActive(true);  // 미리보기 아이템 활성화
            isPreviewActive = true;  // 미리보기 활성화
            SetPreviewItemTransparency(0.3f);  // 미리보기 아이템의 투명도 설정
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

        // 배치할 위치를 결정하는 로직 (미리보기 아이템 위치 설정)
        switch (item.placeType)
        {
            case PlaceType.Floor:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, floorLayer))
                {
                    if(item.name == "Old Chest")
                    {
                        placePos = hit.point + new Vector3(0, 0.5f, 0); // 바닥에서 조금 더 위로 배치
                    }

                    else
                    {
                        placePos = hit.point + new Vector3(0, 0, 0); // 바닥에서 조금 더 위로 배치
                    }
                    // 플레이어와 너무 가까운지 체크하여 최소 거리 설정
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
                placePos = Vector3.zero; // placePos를 기본값으로 초기화

                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, wallLayer))
                {
                    placePos = hit.point;  // 벽에 설치될 위치

                    // 벽에 맞게 회전, 벽에 배치되지만, 플레이어가 바라보는 방향으로 앞을 유지하도록 설정
                    Vector3 wallNormal = hit.normal;  // 벽의 법선 벡터
                    Vector3 playerDirection = playerHand.forward;  // 플레이어의 시선 방향

                    // 플레이어가 바라보는 벽의 '앞'에 배치되도록 설정
                    if (Vector3.Dot(wallNormal, playerDirection) < 0) // 벽의 법선 방향이 플레이어 방향과 반대일 때
                    {
                        placeRot = Quaternion.LookRotation(wallNormal); // 벽의 방향으로 회전 (벽의 앞쪽)
                    }
                    else
                    {
                        placeRot = Quaternion.LookRotation(-wallNormal); // 벽의 반대 방향으로 회전 (벽의 앞쪽)
                    }

                    // 벽에 너무 가까워지지 않도록 플레이어의 위치와의 거리를 고려
                    placePos = hit.point + (wallNormal * 0.07f);  // 벽의 앞쪽에 조금 배치 (0.07f로 설정)

                    // 플레이어와 너무 가까워지지 않도록 0.5f 거리만큼 밀어냄
                    float distanceToPlayer = Vector3.Distance(placePos, playerHand.position);
                    if (distanceToPlayer < 0.5f)
                    {
                        // 아이템이 플레이어에게 너무 가까워지지 않도록 0.5f 거리만큼 밀어냄
                        placePos = playerHand.position + playerHand.forward * 0.5f;  // 최소 거리 0.5 설정
                    }

                    // 벽에만 배치되도록 보장 (벽을 벗어나지 않게)
                    if (Vector3.Distance(placePos, hit.point) > placeDistance)
                    {
                        placePos = hit.point + (wallNormal * 0.07f);  // 벽의 앞쪽에 정확히 배치
                    }

                    // 미리보기 아이템을 설정된 위치와 회전으로 이동
                    if (previewItem != null)
                    {
                        previewItem.SetActive(true);  // 미리보기 아이템 활성화
                        previewItem.transform.position = placePos;
                        previewItem.transform.rotation = placeRot;

                        // 미리보기 아이템과 플레이어 간의 충돌을 무시
                        Collider previewItemCollider = previewItem.GetComponent<Collider>();
                        Collider playerHandCollider = playerHand.GetComponent<Collider>();

                        if (previewItemCollider != null && playerHandCollider != null)
                        {
                            Physics.IgnoreCollision(previewItemCollider, playerHandCollider, true);  // 충돌 무시
                        }
                    }
                }
                else
                {
                    // 벽에 레이가 쏘이지 않으면 미리보기 아이템을 비활성화하지 않고, 위치만 갱신
                    if (previewItem != null)
                    {
                        previewItem.SetActive(true);  // 미리보기 아이템을 계속 활성화
                        previewItem.transform.position = playerHand.position;  // 위치를 플레이어 손 위치로 갱신
                        previewItem.transform.rotation = playerHand.rotation;  // 회전도 갱신
                    }
                }
                break;




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
        if (previewItem == null) return;

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        Vector3 placePos = previewItem.transform.position;
        Quaternion placeRot = previewItem.transform.rotation;

        // 미리보기 아이템의 콜라이더를 비활성화 (배치 전)
        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;  // 콜라이더 비활성화
        }

        // 아이템을 배치할 위치에 인스턴스화
        GameObject placedObject = Instantiate(heldItem, placePos, placeRot);
        placedObject.SetActive(true);  // 배치된 아이템 활성화

        // 배치 후에 콜라이더를 다시 활성화 (배치된 아이템)
        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = true;  // 배치된 아이템의 콜라이더 활성화
        }

        // heldItem을 삭제하고 손에 아이템을 들고 있지 않게 처리
        heldItem = null;

        // 미리보기 아이템 삭제
        Destroy(previewItem);
        previewItem = null;  // 미리보기 아이템 초기화
        isPreviewActive = false;  // 미리보기 비활성화

        // 아이템 배치 정보 업데이트
        UpdatePlacementInfo(item.itemName);
    }



    void UpdatePlacementInfo(string itemName)
    {
        if (itemScores.ContainsKey(itemName))
        {
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
                Material[] materials = renderer.sharedMaterials;

                foreach (Material material in materials)
                {
                    if (material.HasProperty("_Color"))
                    {
                        Color color = material.color;
                        color = new Color(0f / 255f, 255f / 255f, 255f / 255f, alpha); // 파란색 (R=0, G=0, B=1) + 알파 값으로 투명도 설정
                        material.color = color;

                        // 알파값을 변경할 때, 쉐이더가 투명도를 지원하도록 설정
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
}
