using UnityEngine;

public class BlockController : MonoBehaviour
{
    private bool blast = false;
    private bool machine = true; // 초기값 true로 설정
    private bool compressor = false;

    private GameObject[] makingBObjects;
    private GameObject[] makingMObjects;
    private GameObject[] makingCObjects;

    void Start()
    {
        // 태그 오브젝트를 캐싱
        makingBObjects = GameObject.FindGameObjectsWithTag("making_B");
        makingMObjects = GameObject.FindGameObjectsWithTag("making_M");
        makingCObjects = GameObject.FindGameObjectsWithTag("making_C");

        // 초기 상태 체크
        CheckBlockStates();
    }

    void Update()
    {
        // 변수가 변경된 경우에만 상태 체크 (현재는 간단히 매 프레임 체크)
        CheckBlockStates();
    }

    private void CheckBlockStates()
    {
        // blast가 true일 때 making_B 태그 오브젝트의 Block 비활성화
        if (blast)
        {
            foreach (GameObject obj in makingBObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf) // 이미 비활성화되지 않은 경우만 처리
                {
                    block.SetActive(false);
                    Debug.Log($"making_B Block deactivated in {obj.name}");
                }
            }
        }

        // machine가 true일 때 making_M 태그 오브젝트의 Block 비활성화
        if (machine)
        {
            foreach (GameObject obj in makingMObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf)
                {
                    block.SetActive(false);
                    Debug.Log($"making_M Block deactivated in {obj.name}");
                }
            }
        }

        // compressor가 true일 때 making_C 태그 오브젝트의 Block 비활성화
        if (compressor)
        {
            foreach (GameObject obj in makingCObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf)
                {
                    block.SetActive(false);
                    Debug.Log($"making_C Block deactivated in {obj.name}");
                }
            }
        }
    }

    // 자식 오브젝트에서 Block이라는 이름의 오브젝트를 찾는 메서드
    private GameObject FindChildBlock(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == "Block")
            {
                return child.gameObject;
            }
            // 재귀적으로 자식의 자식까지 검색
            GameObject block = FindChildBlock(child);
            if (block != null)
            {
                return block;
            }
        }
        return null;
    }

    // 외부에서 변수 설정용 메서드 (필요 시 사용)
    public void SetBlast(bool value) { if (blast != value) { blast = value; CheckBlockStates(); } }
    public void SetMachine(bool value) { if (machine != value) { machine = value; CheckBlockStates(); } }
    public void SetCompressor(bool value) { if (compressor != value) { compressor = value; CheckBlockStates(); } }
}