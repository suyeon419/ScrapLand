using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject GameObject;

    public PlacementManager PlacementManager;

    // Update is called once per frame
    void Update()
    {
        PlacementManager.SetHeldItem(GameObject);
    }
}
