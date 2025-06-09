using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Header("햇빛 설정")]
    public Light sunLight;
    public Gradient lightColorOverTime;
    public AnimationCurve intensityCurve;

    [Header("하늘 설정")]
    public Material skyboxMaterial;
    public Gradient skyColorOverTime;

    private float totalDayDuration = 10f;//15qns=900 일단 10초해둠
    private float currentTime = 0f;
    private bool dayEnded = false;

    void Start()
    {
        if (sunLight == null)
        {
            Debug.LogError("Sun Light가 설정되지 않았습니다!");
        }
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox Material이 설정되지 않았습니다!");
        }
    }

    void Update()
    {
        if (dayEnded) return;

        currentTime += Time.deltaTime;

        float normalizedTime = currentTime / totalDayDuration;
        UpdateSun(normalizedTime);

        if (currentTime >= totalDayDuration)
        {
            EndDay();
        }
    }

    void UpdateSun(float t)
    {
        sunLight.color = lightColorOverTime.Evaluate(t);
        sunLight.intensity = intensityCurve.Evaluate(t);

        float sunAngle = Mathf.Lerp(-5f, 175f, t); // 해 뜨고 지는 회전
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

        // 하늘 색 조절
        if (skyboxMaterial != null)
        {
            Color skyTint = skyColorOverTime.Evaluate(t);
            skyboxMaterial.SetColor("_SkyTint", skyTint);

            RenderSettings.skybox = skyboxMaterial;
        }
    }

    void EndDay()
    {
        dayEnded = true;
        Debug.Log("하루 끝!");

        DaySummery.instance.EndOneDay();
    }
}
