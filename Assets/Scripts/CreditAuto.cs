using UnityEngine;
using UnityEngine.UI;

public class CreditAuto : MonoBehaviour
{
    [Header("ũ���� �г�")]
    public RectTransform panelRectTransform;
    public float scrollSpeed = 100f; // �ʴ� �̵� �Ÿ�
    private bool isScrolling = false;

    [Header("�ȴ� ĳ���� ������(�̹��� 3��)")]
    public GameObject[] characterFrames; // 3���� ĳ���� �̹��� ������Ʈ
    public float frameInterval = 0.15f; // �� �����Ӵ� �ð�(��)
    private int currentFrame = 0;
    private float frameTimer = 0f;

    void Start()
    {
        // �г� ���� ��ġ ����
        if (panelRectTransform != null)
        {
            Vector3 startPos = panelRectTransform.localPosition;
            startPos.y = -3000f;
            panelRectTransform.localPosition = startPos;
        }
        // ĳ���� ������ �ʱ�ȭ
        ShowFrame(0);
    }

    public void StartScroll()
    {
        isScrolling = true;
    }

    void Update()
    {
        // ũ���� �г� ��ũ��
        if (isScrolling && panelRectTransform != null)
        {
            Vector3 pos = panelRectTransform.localPosition;
            pos.y += scrollSpeed * Time.deltaTime;
            panelRectTransform.localPosition = pos;
        }

        // �ȴ� �ִϸ��̼� ������ ��ȯ
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
