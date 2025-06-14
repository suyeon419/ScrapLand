using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemBoxSpawner : MonoBehaviour
{
    public List<SpawnItem> spawnItems; 
    public int spawnCount = 10;
    public float minDistance = 2f; 
    public float raycastHeight = 5f; 
    public float navMeshRadius = 5f; 

    private Collider myCollider;
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        myCollider = GetComponent<Collider>();
        if (myCollider == null)
        {
            Debug.LogError("������ �ڽ� �ݶ��̴� ���� X");
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

            // NavMesh ���� �ִ��� Ȯ�� �߰�
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

        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
        {
            if (col.bounds.Contains(hit.point))
            {
                return hit.point;
            }
        }

        return col.transform.position; // ���н� Collider �߽� ��ȯ
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
