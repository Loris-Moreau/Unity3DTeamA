using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletsInventory : MonoBehaviour
{
    private int load;
    private int counter;
    [SerializeField]
    private TextMeshPro textCounter;
    [SerializeField]
    private TextMeshPro textLoad;


    public static BulletsInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        textLoad.text = load.ToString();
        textCounter.text = counter.ToString();
    }

    public void Shoot()
    {

    }
}
