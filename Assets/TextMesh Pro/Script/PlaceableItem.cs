using UnityEngine;
using System;
using System.Collections.Generic;

public enum PlaceType { Floor, Wall, Ceiling }

public class PlaceableItem : MonoBehaviour
{
    public string itemName;
    public PlaceType placeType;
}