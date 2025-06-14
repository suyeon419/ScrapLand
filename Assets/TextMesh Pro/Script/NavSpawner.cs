using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnItem
{
    public GameObject prefab;
    [Range(0, 100)]
    public float spawnChance; // 확률
}

public class NavSpawner : MonoBehaviour
{
    public List<SpawnItem> spawnItems; // 아이템과 확률 목록
    public int spawnCount = 100;
    public float spawnRange = 50f; // 센터 기준 반경 내에서 아이템 생성
    public float minDistance = 2f; // 아이템 떨어져야 할 거리
    public LayerMask itemBoxLayer;
    public Transform centerPoint; // 센터 지정

    private Collider[] itemBoxColliders;
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Awake()
    {
        if (centerPoint == null)
        {
            GameObject obj = GameObject.Find("Map Portal pos");
            if (obj != null)
                centerPoint = obj.transform;
            else
                Debug.LogWarning("Map Portal pos를 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        itemBoxColliders = FindObjectsOfType<Collider>();
        SpawnOnNavMesh();
    }

    void SpawnOnNavMesh()
    {
        if (centerPoint == null)
        {
            Debug.LogError("CenterPoint를 설정하세요!");
            return;
        }

        spawnPositions.Clear();
        int attempt = 0;
        int maxAttempts = spawnCount * 50; // 실패 대비 여유 카운트

        while (spawnPositions.Count < spawnCount && attempt < maxAttempts)
        {
            attempt++;

            Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * spawnRange;
            randomPoint.y = centerPoint.position.y; // 높이 고정

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit navHit, 5.0f, NavMesh.AllAreas))
            {
                Vector3 candidate = navHit.position;

                if (!IsOverlapItemBox(candidate) && IsFarFromOthers(candidate, spawnPositions))
                {
                    spawnPositions.Add(candidate);
                }
            }
        }

        if (spawnPositions.Count < spawnCount)
        {
            Debug.LogWarning($"필요한 {spawnCount}개 중 {spawnPositions.Count}개만 생성했습니다.");
        }

        foreach (var pos in spawnPositions)
        {
            GameObject selectedPrefab = SelectPrefab();
            Instantiate(selectedPrefab, pos, Quaternion.identity);
        }

        Debug.Log($"아이템 {spawnPositions.Count}/{spawnCount}개 생성 완료.");
    }

    bool IsOverlapItemBox(Vector3 position)
    {
        foreach (var col in itemBoxColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("ItemBox"))
            {
                if (col.bounds.Contains(position))
                    return true;
            }
        }
        return false;
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

    GameObject SelectPrefab()
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

        // 만약 실패하면 마지막 아이템 반환
        return spawnItems[spawnItems.Count - 1].prefab;
    }
}
