using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using InventorySystem;
using UnityEngine.UI;

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
            bool willOpen = !optionPanel.activeSelf;

            if (optionPanel.activeSelf) 
            {
                GameManager_ScrapLand.instance.SetSensOrigin();
            }
            else
            {
                StopCamMoving();
            }
            optionPanel.SetActive(!optionPanel.activeSelf);
            wInven(willOpen);

        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool willOpen = !TutoPanel.activeSelf;

            if (TutoPanel.activeSelf)
            {
                GameManager_ScrapLand.instance.SetSensOrigin();
            }
            else
            {
                StopCamMoving();
            }
            TutoPanel.SetActive(!TutoPanel.activeSelf);

            wInven(willOpen);

        }

        if (!mainBtn.activeSelf && SceneManager.GetActiveScene().buildIndex != 0)
        {
            mainBtn.SetActive(true);
        }
    }

    public void wInven(bool willOpen)
    {
        if (!SceneManager.GetActiveScene().name.Equals("PlayScene")) return;
        if (PlayerInvenManager.instance.InvenMode)
        {
            PlayerInvenManager.instance.Inventory_Cloth.gameObject.SetActive(!willOpen);
            PlayerInvenManager.instance.Inven_Background.gameObject.SetActive(!willOpen);
            if (PlayerInvenManager.instance.IsBagOn)
            {
                PlayerInvenManager.instance.InventoryUI.gameObject.SetActive(!willOpen);
            }
        }
        if (StorageManager.instance.isStorageOpen)
        {
            StorageManager.instance.Storage1.gameObject.SetActive(!willOpen);
            PlayerInvenManager.instance.HotBar_Bar.gameObject.SetActive(!willOpen);

        }
        else
        {
            PlayerInvenManager.instance.HotBar_Bar.gameObject.SetActive(!willOpen);
            PlayerInvenManager.instance.HotBar_Background.gameObject.SetActive(!willOpen);
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
