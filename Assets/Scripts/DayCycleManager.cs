using TMPro;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Header("블렌드 스카이박스 머티리얼")]
    public Material blendedSkyboxMaterial; // SkyboxBlended 셰이더 적용 머티리얼

    [Header("태양(Direct Light)")]
    public Light sunLight; // Directional Light 연결

    public float totalDayDuration = 10f;   // 하루 길이(초)
    private float currentTime = 0f;
    private bool dayEnded = false;

    // 태양 궤적 설정
    public float sunMinAngle = -5f;   // 해가 지면 아래에 있을 때 각도
    public float sunMaxAngle = 175f;  // 해가 하늘 가장 높이 있을 때 각도
    public float sunTilt = 0f;        // 해 궤적의 Y축 기울기 (동→서 방향)
    [Header("일차&시간 흐름")]
    public RectTransform needleTransform;
    public TextMeshProUGUI dayText;

    void Start()
    {
        if (blendedSkyboxMaterial == null)
            Debug.LogError("Blended Skybox Material이 설정되지 않았습니다!");
        if (sunLight == null)
            Debug.LogError("Directional Light가 설정되지 않았습니다!");
        RenderSettings.skybox = blendedSkyboxMaterial;
        dayText.text = "| "+GameManager_ScrapLand.instance.GetDayNum() + "일차";
    }

    void Update()
    {
        if (dayEnded) return;

        currentTime += Time.deltaTime;
        float t = Mathf.Clamp01(currentTime / totalDayDuration);

        // ---- Skybox Blend ----
        float blend = 0f;
        if (t < 0.15f)
        {
            // 이른 아침: 밤(1) → 낮(0)으로 천천히 Blend
            blend = Mathf.Lerp(1f, 0f, t / 0.15f);
        }
        else if (t < 0.7f)
        {
            // 대낮: 낮 Skybox만 보임
            blend = 0f;
        }
        else
        {
            // 밤: 낮(0) → 밤(1)으로 천천히 Blend
            blend = Mathf.Lerp(0f, 1f, (t - 0.7f) / 0.3f);
        }
        blendedSkyboxMaterial.SetFloat("_Blend", blend);

        // ---- 자연스러운 태양 궤적 ----
        // 하루 전체를 0~1로 정규화하여 Sine 곡선으로 태양의 움직임을 만듦
        // Sine(-90°) = -1 (지면), Sine(90°) = +1 (지면 반대편)
        float sunRadians = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, t);
        float sunCurve = (Mathf.Sin(sunRadians) + 1f) / 2f; // 0~1
        float sunAngle = Mathf.Lerp(sunMinAngle, sunMaxAngle, sunCurve);
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, sunTilt, 0);

        // 해가 지면 아래에 있으면 꺼주기
        sunLight.enabled = sunAngle > 0f;

        if (needleTransform != null)
        {
            float angle = 360f * t;
            needleTransform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }


        if (currentTime >= totalDayDuration)
            EndDay();
    }

    void EndDay()
    {
        dayEnded = true;
        Debug.Log("하루 끝!");
        DaySummery.instance.EndOneDay();
    }
}
