using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletsInventory : MonoBehaviour
{
    [HideInInspector]
    public int load;
    public int reloadNumber;
    public int counter;
    [SerializeField]
    private TextMeshProUGUI textBullet;
    [SerializeField]
    private GameObject textReload;



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
        load = reloadNumber;
        textBullet.text = load.ToString() + " / " + counter.ToString();
        textReload.SetActive(false);
    }

    public void ShootInventory()
    {
        load--;
        if (load == 0)
        {
            textBullet.color = new Color32(255, 0, 0, 255);
            textReload.SetActive(true);
        }
            textBullet.text = load.ToString() + " / " + counter.ToString();
    }

    public void ReloadInventory()
    {
        if(counter == 0) return;
        if (load == reloadNumber) return;
        if (counter < reloadNumber - load)
        {
            load += counter;
            counter = 0;
        }
        else
        {
            counter -= reloadNumber;
            counter += load;
            load = reloadNumber;
        }
        textReload.SetActive(false);
        textBullet.text = load.ToString() + " / " + counter.ToString();
        textBullet.color = new Color32(255, 255, 255, 255);
    }
}
