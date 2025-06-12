using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool test = false;

    void Update()
    {
        if (!false)
        {
            PlacementManager.Instance.SetHeldItem("Glass Pot");
            test = true;
        }
    }
}
