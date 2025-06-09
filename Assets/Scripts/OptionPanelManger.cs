using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            TutoPanel.SetActive(!TutoPanel.activeSelf);
        }

        if(!mainBtn.activeSelf && SceneManager.GetActiveScene().buildIndex != 0)
        {
            mainBtn.SetActive(true);
        }
    }

    public void ActiveOptionPanel(bool active = true)
    {
        optionPanel.SetActive(active);
    }

    public void ActiveTutoPanel(bool active = true)
    {
        TutoPanel.SetActive(active);
    }
}
