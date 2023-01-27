using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletsInventory : MonoBehaviour
{
    public int load;
    public int reloadNumber;
    public int counter;
    [SerializeField]
    private TextMeshProUGUI textBullet;


    public static BulletsInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de BulletsInventory dans la scène");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        textBullet.text = load.ToString() + " / " + counter.ToString();
    }

    public void ShootInventory()
    {
        load--;
        ReloadInventory();
        textBullet.text = load.ToString() + " / " + counter.ToString();
    }

    private void ReloadInventory()
    {
        if(load == 0)
        {
            if(counter >= reloadNumber)
            {
                load = reloadNumber;
            }
            else
            {
                load = counter;
            }
            counter -= reloadNumber;
            if (counter < 0)
            {
                counter = 0;
            }
        }
    }
}
