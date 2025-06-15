using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GlobalCanvasManager : MonoBehaviour
{
    public static GlobalCanvasManager instance;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject mainBtn;
    [SerializeField] GameObject TutoPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        optionPanel.SetActive(false);
        TutoPanel.SetActive(false);
    }

    void Update()
    {
        if (DaySummery.instance != null)
        {
            if (DaySummery.instance.IsActiveSummeryPannel())
            {
                StopCamMoving();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf) 
            {
                GameManager_ScrapLand.instance.SetSensOrigin();
            }
            else
            {
                StopCamMoving();
            }
            optionPanel.SetActive(!optionPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (TutoPanel.activeSelf) 
            {
                GameManager_ScrapLand.instance.SetSensOrigin();
            }
            else
            {
                StopCamMoving();
            }
            TutoPanel.SetActive(!TutoPanel.activeSelf);
        }

        if(!mainBtn.activeSelf && SceneManager.GetActiveScene().buildIndex != 0)
        {
            mainBtn.SetActive(true);
        }
    }

    public void ActiveOptionPanel(bool active = true)
    {
        StopCamMoving();
        optionPanel.SetActive(active);
    }

    public void ActiveTutoPanel(bool active = true)
    {
        StopCamMoving();
        TutoPanel.SetActive(active);
    }

    public void StopCamMoving()
    {
        Camera mainCam = Camera.main;
        ThirdPersonCamera camera = mainCam.GetComponent<ThirdPersonCamera>();
        if (camera != null)
        {
            camera.SetSensitivity(0);
        }
    }
}
