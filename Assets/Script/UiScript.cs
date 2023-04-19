using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UiScript : MonoBehaviour
{
    public static UiScript instance;

    #region Ui
    public int textTimer;

    [Space]
    [Header("Door Locked Message")]
    [Space]
    public GameObject messageDoorLockedGameObj;
    public TextMeshProUGUI messageDoorLockedTMP;


    [Space]
    [Header("Texte Inventaire Plein")]
    [Space]
    public GameObject fullInventoryGameObj;
    public TextMeshProUGUI fullInventoryTMP;
  
    [TextArea]
    public string txtFullAmmo, txtFullMedikit;

    [Space]
    [Header("Texte Interaction")]
    [Space]
    public GameObject interactMessageGameObj;
    public TextMeshProUGUI interactMessageTMP;

    [TextArea]
    public string txtBed, txtMedikit, txtAmmo, txtDoor, txtLockedDoorInfo, txtLockedAndTry;

    #endregion

    private void Awake()
    {
        if (instance) Destroy(this);
        instance = this;
    }

    private void Start()
    {
        RemoveText();
    }

    public void FullInventory()
    {
        //fullInventoryGameObj.SetActive(true);
        if (Controller.Instance.isMedikit)
        {
            fullInventoryTMP.text = txtFullMedikit;
        }
        else if (Controller.Instance.IsAmmo)
        {
            fullInventoryTMP.text = txtFullAmmo;
        }
        Invoke("RemoveText", textTimer);
    }

    public void InteractMessage()
    {
        interactMessageGameObj.SetActive(true);
        if (Controller.Instance.isMedikit)
        {
            interactMessageTMP.text = txtMedikit;
        }
        else if (Controller.Instance.IsAmmo)
        {
            interactMessageTMP.text = txtAmmo;
        }
        else if (Controller.Instance.isBed)
        {
            interactMessageTMP.text = txtBed;
        }
        else if (Controller.Instance.isDoor)
        {
            interactMessageTMP.text = txtDoor;
        }
        else if (Controller.Instance.isDoorLocked)
        {
            interactMessageTMP.text = txtLockedDoorInfo;
        }
        
    }
    public void DoorLockedMessage()
    {
        if(Controller.Instance.isLockedAndUTry)
        {
            messageDoorLockedGameObj.SetActive(true);
            messageDoorLockedTMP.text = txtLockedAndTry;
            Invoke("RemoveText", textTimer);
        }
    }

    public void RemoveText()
    {
        //fullInventoryGameObj.SetActive(false);

        interactMessageGameObj.SetActive(false);

        messageDoorLockedGameObj.SetActive(false);
    }



}

