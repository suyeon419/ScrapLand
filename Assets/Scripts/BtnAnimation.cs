using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

enum btnType
{
    Start,
    Load,
    End,
    Setting,
    Tutorial,
    Back
}

public class BtnAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float scaleChange = 1.1f;
    [SerializeField] btnType btnType;
    [SerializeField] string LoadScene = "PlayScene";

    private Vector3 defaultScale;
    private void Start()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = defaultScale * scaleChange;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(SoundManager.instance)
            SoundManager.instance.PlaySound("Click");
        switch (btnType)
        {
            case btnType.Start:
                if (GameManager_ScrapLand.instance.GetDayNum() == 1)
                {
                    SceneManager.LoadScene("TutoScene");
                }
                else
                {
                    SceneManager.LoadScene("PlayScene");
                }
                break;
            case btnType.Load:
                if(GlobalCanvasManager.instance != null)
                {
                    GlobalCanvasManager.instance.ActiveOptionPanel(false);
                    GlobalCanvasManager.instance.ActiveTutoPanel(false);
                }
                SceneManager.LoadScene(LoadScene);
                break;
            case btnType.End:
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                break;
            case btnType.Setting:
                GlobalCanvasManager.instance.ActiveOptionPanel();
                break;
            case btnType.Tutorial:
                GlobalCanvasManager.instance.ActiveTutoPanel();
                break;
            case btnType.Back:
                transform.parent.gameObject.SetActive(false);
                GameManager_ScrapLand.instance.SetSensOrigin();
                break;

        }
    }
}
