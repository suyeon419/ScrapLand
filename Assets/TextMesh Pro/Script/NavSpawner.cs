using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnItem
{
    public GameObject prefab;
    [Range(0, 100)]
    public float spawnChance; // Ȯ��
}

public class NavSpawner : MonoBehaviour
{
    public List<SpawnItem> spawnItems; // �����۰� Ȯ�� ���
    public int spawnCount = 100;
    public float spawnRange = 50f; // ���� ���� �ݰ� ������ ������ ����
    public float minDistance = 2f; // ������ �������� �� �Ÿ�
    public LayerMask itemBoxLayer;
    public Transform centerPoint; // ���� ����

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
                Debug.LogWarning("Map Portal pos�� ã�� �� �����ϴ�.");
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
            Debug.LogError("CenterPoint�� �����ϼ���!");
            return;
        }

        spawnPositions.Clear();
        int attempt = 0;
        int maxAttempts = spawnCount * 50; // ���� ��� ���� ī��Ʈ

        while (spawnPositions.Count < spawnCount && attempt < maxAttempts)
        {
            attempt++;

            Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * spawnRange;
            randomPoint.y = centerPoint.position.y; // ���� ����

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
            Debug.LogWarning($"�ʿ��� {spawnCount}�� �� {spawnPositions.Count}���� �����߽��ϴ�.");
        }

        foreach (var pos in spawnPositions)
        {
            GameObject selectedPrefab = SelectPrefab();
            Instantiate(selectedPrefab, pos, Quaternion.identity);
        }

        Debug.Log($"������ {spawnPositions.Count}/{spawnCount}�� ���� �Ϸ�.");
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

        // ���� �����ϸ� ������ ������ ��ȯ
        return spawnItems[spawnItems.Count - 1].prefab;
    }
}
