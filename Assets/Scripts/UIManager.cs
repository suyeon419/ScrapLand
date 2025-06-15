using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<GameObject> uiPanels;

    void Awake()
    {
        Instance = this;
    }

    public void CloseAllPanels()
    {
        GlobalCanvasManager.instance.ActiveOptionPanel(false);
        GlobalCanvasManager.instance.ActiveTutoPanel(false);
        foreach (var panel in uiPanels)
            panel.SetActive(false);
    }

    public void OpenPanel(GameObject panel)
    {
        CloseAllPanels();
        panel.SetActive(true);
    }
}
