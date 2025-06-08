using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemBoxSpawner : MonoBehaviour
{
    public List<SpawnItem> spawnItems; // 이 콜라이더 안에서 스폰할 아이템들
    public int spawnCount = 10;
    public float minDistance = 2f; // 아이템들 간 거리 제한
    public float raycastHeight = 5f; // 위에서 Raycast 쏠 높이
    public float navMeshRadius = 5f; // NavMesh 근처 허용 반경

    private Collider myCollider;
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        myCollider = GetComponent<Collider>();
        if (myCollider == null)
        {
            Debug.LogError("아이템 박스 콜라이더 설정 X");
            return;
        }

        SpawnItemBox();
    }

    void SpawnItemBox()
    {
        spawnPositions.Clear();
        int attempt = 0;
        int maxAttempts = spawnCount * 100;

        while (spawnPositions.Count < spawnCount && attempt < maxAttempts)
        {
            attempt++;

            Vector3 randomPoint = GetRandomPoint(myCollider);

            // NavMesh 위에 있는지 확인 추가
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit navHit, navMeshRadius, NavMesh.AllAreas))
            {
                Vector3 candidate = navHit.position;

                Bounds expandedBounds = myCollider.bounds;
                expandedBounds.Expand(0.5f);

                if (expandedBounds.Contains(candidate) && IsFarFromOthers(candidate, spawnPositions))
                {
                    spawnPositions.Add(candidate);
                }
            }
        }

        foreach (var pos in spawnPositions)
        {
            GameObject selectedPrefab = SelectRandomPrefab();
            Instantiate(selectedPrefab, pos, selectedPrefab.transform.rotation); 
        }

        Debug.Log($"{gameObject.name} 에서 {spawnPositions.Count}/{spawnCount}개 아이템 생성 완료.");
    }

    Vector3 GetRandomPoint(Collider col)
    {
        Vector3 min = col.bounds.min;
        Vector3 max = col.bounds.max;

        Vector3 randomPoint = new Vector3(
            Random.Range(min.x, max.x),
            max.y + raycastHeight,
            Random.Range(min.z, max.z)
        );

        // 아래로 Raycast를 쏴서 바닥 위치를 정확히 찾음
        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
        {
            if (col.bounds.Contains(hit.point))
            {
                return hit.point;
            }
        }

        return col.transform.position; // 실패시 Collider 중심 반환
    }

    bool IsFarFromOthers(Vector3 candidate, List<Vector3> existing)
    {
        foreach (var pos in existing)
        {
            if (Vector3.Distance(pos, candidate) < minDistance)
                return false;
        }
        return true;
    }

    GameObject SelectRandomPrefab()
    {
        float totalChance = 0f;
        foreach (var item in spawnItems)
        {
            totalChance += item.spawnChance;
        }

        float randomPoint = Random.Range(0f, totalChance);
        float current = 0f;

        foreach (var item in spawnItems)
        {
            current += item.spawnChance;
            if (randomPoint <= current)
            {
                return item.prefab;
            }
        }

        return spawnItems[spawnItems.Count - 1].prefab;
    }
}
