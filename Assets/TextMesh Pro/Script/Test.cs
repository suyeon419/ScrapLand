using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool test = false;

    void Update()
    {
        if (!test)
        {
            PlacementManager.Instance.SetHeldItem("Table");
            test = true;
        }
    }
}
