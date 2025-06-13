using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HappyEarth : MonoBehaviour
{
    private Slider happySlider;
    private Slider preview;

    public static HappyEarth instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        happySlider = transform.Find("realgage").GetComponent<Slider>();
        preview = transform.Find("preview").GetComponent<Slider>();

        if (happySlider == null || preview == null)
        {
            Debug.LogError("슬라이더가 할당되지 않았습니다!");
        }

        if (GameManager_ScrapLand.instance != null)
        {
            GameManager_ScrapLand.instance.ApplyHappyEarthGageOnLoad();
        }
    }

    public int GetHappyGage()
    {
        return (int)preview.value;
    }

    public void setRealGage(int value)
    {
        happySlider.value = value;
        preview.value = happySlider.value;
    }

    public void plus_debugiing(int value)
    {
        preview.value += value;
    }

    public void GageUp_Install(string name)
    {
        if (name.Equals("Bench"))
        {
            Install_Interior(name, 10, new Vector3(0,0,0), new Vector3(0,0,0));
        }
        else if(name.Equals("Can Pot"))
        {
            Install_Interior(name, 20, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        }
    }

    private void PlusGageBySale(string name, int value) // 판매 조건검사는 불필요, 검사 후 넘어올 것
    {
        preview.value += value;
    }

    public void Install_Interior(string name, int value, Vector3 pos, Vector3 rot)
    {
        Debug.Log("인테리어 : "+name+" "+value+""+pos+" "+rot);
        GameManager_ScrapLand.instance.New_Install_Interior(name, pos, rot);
        if (GameManager_ScrapLand.instance.IsHappyGageAvailable_install(name))
        {
            preview.value += value;
        }
    }
}
