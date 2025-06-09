using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    public Slider BrightSlider;

    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    AutoExposure exposure;

    void Awake()
    {
        brightness.TryGetSettings(out exposure);
        BrightSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    public void AdjustBrightness(float value)
    {
        if(value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 1f;
        }
    }

    public void OnSliderChanged(float value)
    {
        GameManager_ScrapLand.instance?.SetBrightness(value);
    }

}
