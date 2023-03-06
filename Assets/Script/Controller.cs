using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    #region Movement
    [Space]
    [Header("Movements")]
    [Space]

    private float speed;
    public float speedWalk;
    public float speedRun;
    public float speedCrounch; 
    private Vector2 direction;
    #endregion

    #region Ui
    [Space]
    [Header("Texte Inventaire Plein")]
    [Space]
    public GameObject fullInventoryGO;
    public TextMeshProUGUI fullInventoryTMP;
    [TextArea]
    public string txtFullAmmo, txtFullMedikit;

    [Space]
    [Header("Texte Interaction")]
    [Space]
    public GameObject interactMessage;
    public TextMeshProUGUI interactionMsg;
    [TextArea]
    public string txtBed, txtMedikit, txtAmmo, txtLockedDoor, txtDoor;

    #endregion

    #region Medikit

    [Space]
    [Header("Medikit")]
    [Space]

    public int medikit;
    [Space]
    public GameObject actualMedikit;
    public bool isMedikit;
    public int maxMedikit = 5;

    public int textTimer;
    public int heal = 30;

    #endregion

    #region Bullets
    [Space]
    [Header("Ammo")]
    [Space]
    public bool IsAmmo;
    public GameObject actualAmmo;

    #endregion

    #region Spawn
    [Space]
    [Header("Spawn")]
    [Space]

    public Transform respawnPoint;
    private Transform currentBed;
    
    public bool isBed = false;
    #endregion

    #region Interactions
    [Space]
    [Header("Interact")]
    [Space]

    public FadeOutSleeping fade;

    [Space]
    [Header("Door")]
    [Space]

    public TextMeshProUGUI textDoorIsLocked;

    public bool isDoor = false;
    public bool isDoorLocked = false;

    public Animator DoorAnim;
    #endregion

    private void Awake()
    {
        if(Instance) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        speed = speedWalk;

        medikit = 1;

        transform.position = respawnPoint.position;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        transform.rotation = Quaternion.Euler(0, 0, 0);

        RemoveText();

        DoorAnim.SetBool("IsClosed", true);
        DoorAnim.SetBool("Open", false);
    }

    private void Update()
    {
        ///mouvement
        ///
        if (direction.y != 0)
        {
            transform.position += transform.forward * direction.y * speed * Time.deltaTime;
        }
        if (direction.x != 0)
        {
            transform.position += transform.right * direction.x * speed * Time.deltaTime;
        }
        ///

        /*
        if (textTimer == 0)
        {
            RemoveText();
        }
        */

        /*if (DoorAnim.GetBool("IsClosed"))
        {
            DoorAnim.SetBool("IsClosed", false);
        }
        else 
        if (!DoorAnim.GetBool("IsClosed"))
        {
            DoorAnim.SetBool("IsClosed", true);
        }*/
    }

    public void UpdateRotation(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        /*Debug.Log("DELTA "+mouseDelta);
        Debug.Log("rot = "+transform.rotation.eulerAngles);*/

        if (Mathf.Abs(mouseDelta.x) !=0 )
        {
            transform.rotation *= Quaternion.Euler(0, 0.5f * mouseDelta.x, 0);
        }
        if (mouseDelta.y > 0 && (transform.rotation.eulerAngles.x < 90 || transform.rotation.eulerAngles.x > 315) 
            || mouseDelta.y < 0 && (transform.rotation.eulerAngles.x < 45 || transform.rotation.eulerAngles.x > 270))
        {
            
            transform.rotation *= Quaternion.Euler(0.5f * -mouseDelta.y, 0, 0);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        //Debug.Log(direction);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("touche sprint ");
            speed = speedRun;
        }
        else if (context.canceled)
        {
            speed = speedWalk;
        }
    }
    public void Crounch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = speedCrounch;

        }
        else if (context.canceled)
        {
            speed = speedWalk;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && isBed)
        {
            respawnPoint = currentBed;
            fade.FadeIn();
        }
        else if (context.performed && isMedikit)
        {
            if (medikit < 5)
            {
                medikit++; 
                RemoveText();
                Destroy(actualMedikit);

            }
            else
            {
                fullInventoryGO.SetActive(true);
                fullInventoryTMP.text = txtFullMedikit;
                Invoke("RemoveText", textTimer);
            }
        }
        else if (context.performed && IsAmmo)
        {
            if (BulletsInventory.instance.counter <= BulletsInventory.instance.maxCounter)
            {
                BulletsInventory.instance.AddInventory();
                RemoveText();
                Destroy(actualAmmo);
            }
            else
            {
                fullInventoryGO.SetActive(true);
                fullInventoryTMP.text = txtFullAmmo;
                Invoke("RemoveText", textTimer);
            }
        }
        else if (context.performed && isDoor || isDoorLocked)
        {
            if (isDoorLocked)
            {
                //door can't be opened
                interactMessage.SetActive(false);

                textDoorIsLocked.enabled = true;

                Invoke("RemoveText", textTimer);
            }
            else
            {
                //door opens
                interactMessage.SetActive(false);

                //Debug.Log("Door Opens");

                DoorAnim.gameObject.SetActive(true);
                DoorAnim.SetTrigger("Open");
                
                if (DoorAnim.GetBool("IsClosed"))
                {
                    DoorAnim.SetBool("IsClosed", false);
                }
                else DoorAnim.SetBool("IsClosed", true);
            }
        }
    }
    public void Healing(InputAction.CallbackContext context)
    {
        if (context.performed && PlayerLife.instance.health < PlayerLife.instance.maxHealth && medikit > 0)
        {
            medikit--;
            PlayerLife.instance.HealPlayer(heal);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Bed")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = txtBed;
            isBed = true;
            currentBed = collision.transform;
        }
        else if (collision.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = txtMedikit;

            isMedikit = true;
            actualMedikit = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Ammo")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = txtAmmo;
            IsAmmo = true;
            actualAmmo = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Door")
        {
            isDoorLocked = false;
            isDoor = true;

            DoorAnim = collision.GetComponent<Animator>();

            textDoorIsLocked.enabled = false;
            interactMessage.SetActive(true);
        }
        else if (collision.gameObject.tag == "LockedDoor")
        {
            interactMessage.SetActive(true);
            isDoorLocked = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bed"))
        {
            RemoveText();
            isBed = false;
        }
        else if (other.gameObject.CompareTag("Medikit"))
        {
            RemoveText();
            isMedikit = false;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            RemoveText();

            isDoor = false;
            isDoorLocked = false;

            DoorAnim.SetBool("Open", false);
            DoorAnim.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("LockedDoor"))
        {
            RemoveText();

            isDoor = false;
            isDoorLocked = false;
        }
        else if (other.gameObject.tag == "Ammo")
        {
            RemoveText();
            IsAmmo = false;
        }
    }

    //removes all pop up text that are on screen
    void RemoveText()
    {
        fullInventoryGO.SetActive(false);

        textDoorIsLocked.enabled = false;

        interactMessage.SetActive(false);
    }
}
