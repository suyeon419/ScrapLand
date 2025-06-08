using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClothManager : MonoBehaviour
{
    public GameObject[] cloths; // Array to hold the cloth objects

    public Sprite clothsprite; //ø  ¿Â¬¯Ω√ Ω∫«¡∂Û¿Ã∆Æ
    private void Awake()
    {
        cloths = new GameObject[transform.childCount]; // Initialize the array with the number of child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            cloths[i] = transform.GetChild(i).gameObject; // Store each child object in the array
        }
        
        cloths[5].SetActive(false); // Store each child object in the array
        cloths[7].SetActive(false);
        cloths[9].SetActive(false);
        cloths[10].SetActive(false);
        cloths[11].SetActive(false);
        //E5 Gloves
        //E7 Hat
        //E9 Outerwear
        //10 Pants
        //E11 Shoes

    }

    public void SetClothActive(int index)
    {
        if (index >= 0 && index < cloths.Length)
        {
            cloths[index].SetActive(true); // Set the active state of the specified cloth object
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    public void SetClothInactive(int index)
    {
        if (index >= 0 && index < cloths.Length)
        {
            cloths[index].SetActive(false); // Set the active state of the specified cloth object
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }
}
