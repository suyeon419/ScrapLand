using UnityEngine;
using UnityEngine.UI;

public class CreditAuto : MonoBehaviour
{
    [Header("크레딧 패널")]
    public RectTransform panelRectTransform;
    public float scrollSpeed = 100f; // 초당 이동 거리
    private bool isScrolling = false;

    [Header("걷는 캐릭터 프레임(이미지 3개)")]
    public GameObject[] characterFrames; // 3개의 캐릭터 이미지 오브젝트
    public float frameInterval = 0.15f; // 한 프레임당 시간(초)
    private int currentFrame = 0;
    private float frameTimer = 0f;

    void Start()
    {
        // 패널 시작 위치 세팅
        if (panelRectTransform != null)
        {
            Vector3 startPos = panelRectTransform.localPosition;
            startPos.y = -3000f;
            panelRectTransform.localPosition = startPos;
        }
        // 캐릭터 프레임 초기화
        ShowFrame(0);
    }

    public void StartScroll()
    {
        isScrolling = true;
    }

    void Update()
    {
        // 크레딧 패널 스크롤
        if (isScrolling && panelRectTransform != null)
        {
            Vector3 pos = panelRectTransform.localPosition;
            pos.y += scrollSpeed * Time.deltaTime;
            panelRectTransform.localPosition = pos;
        }

        // 걷는 애니메이션 프레임 전환
        if (characterFrames != null && characterFrames.Length > 0)
        {
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameInterval)
            {
                frameTimer = 0f;
                currentFrame = (currentFrame + 1) % characterFrames.Length;
                ShowFrame(currentFrame);
            }
        }
    }

    void ShowFrame(int idx)
    {
        for (int i = 0; i < characterFrames.Length; i++)
        {
            if (characterFrames[i] != null)
                characterFrames[i].SetActive(i == idx);
        }
    }
}
