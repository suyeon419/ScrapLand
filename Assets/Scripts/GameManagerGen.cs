using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGen : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    void Awake()
    {
        if (GameManager_ScrapLand.instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}
