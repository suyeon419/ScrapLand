using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SensitivitySlider : MonoBehaviour, IPointerUpHandler
{
    public Slider sensSlider;

    public void OnPointerUp(PointerEventData eventData)
    {
        float value = sensSlider.value;
        float roundedValue = Mathf.Round(value * 10f) / 10f;

        GameManager_ScrapLand.instance.SetSensitivity(roundedValue);
    }
}
