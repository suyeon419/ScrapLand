using TMPro;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Header("���� ��ī�̹ڽ� ��Ƽ����")]
    public Material blendedSkyboxMaterial; // SkyboxBlended ���̴� ���� ��Ƽ����

    [Header("�¾�(Direct Light)")]
    public Light sunLight; // Directional Light ����

    public float totalDayDuration = 10f;   // �Ϸ� ����(��)
    private float currentTime = 0f;
    private bool dayEnded = false;

    // �¾� ���� ����
    public float sunMinAngle = -5f;   // �ذ� ���� �Ʒ��� ���� �� ����
    public float sunMaxAngle = 175f;  // �ذ� �ϴ� ���� ���� ���� �� ����
    public float sunTilt = 0f;        // �� ������ Y�� ���� (���漭 ����)
    [Header("����&�ð� �帧")]
    public RectTransform needleTransform;
    public TextMeshProUGUI dayText;

    void Start()
    {
        if (blendedSkyboxMaterial == null)
            Debug.LogError("Blended Skybox Material�� �������� �ʾҽ��ϴ�!");
        if (sunLight == null)
            Debug.LogError("Directional Light�� �������� �ʾҽ��ϴ�!");
        RenderSettings.skybox = blendedSkyboxMaterial;
        dayText.text = "| "+GameManager_ScrapLand.instance.GetDayNum() + "����";
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
            // �̸� ��ħ: ��(1) �� ��(0)���� õõ�� Blend
            blend = Mathf.Lerp(1f, 0f, t / 0.15f);
        }
        else if (t < 0.7f)
        {
            // �볷: �� Skybox�� ����
            blend = 0f;
        }
        else
        {
            // ��: ��(0) �� ��(1)���� õõ�� Blend
            blend = Mathf.Lerp(0f, 1f, (t - 0.7f) / 0.3f);
        }
        blendedSkyboxMaterial.SetFloat("_Blend", blend);

        // ---- �ڿ������� �¾� ���� ----
        // �Ϸ� ��ü�� 0~1�� ����ȭ�Ͽ� Sine ����� �¾��� �������� ����
        // Sine(-90��) = -1 (����), Sine(90��) = +1 (���� �ݴ���)
        float sunRadians = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, t);
        float sunCurve = (Mathf.Sin(sunRadians) + 1f) / 2f; // 0~1
        float sunAngle = Mathf.Lerp(sunMinAngle, sunMaxAngle, sunCurve);
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, sunTilt, 0);

        // �ذ� ���� �Ʒ��� ������ ���ֱ�
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
        Debug.Log("�Ϸ� ��!");
        DaySummery.instance.EndOneDay();
    }
}
