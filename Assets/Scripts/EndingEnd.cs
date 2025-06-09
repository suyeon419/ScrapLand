using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingEnd : MonoBehaviour
{
    private bool isEnd = false;
    void Update()
    {
        if(isEnd)
            SceneManager.LoadScene("MainScene");
    }

    public void EndGame()
    {
        isEnd= true;
    }
}
