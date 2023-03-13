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

    public float speed;
    public float speedWalk = 5;
    public float speedRun = 7;
    public float speedCrounch = 2; 
    private Vector2 direction;
    #endregion

    #region Camera
    public Transform playerEyePos;
    public Camera FPSCam;
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

    public bool isDoor = false;
    public bool isDoorLocked = false;
    public bool isLockedAndUTry = false;

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

        FPSCam.transform.position = playerEyePos.position;
    }

    public void UpdateRotation(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        if (Mathf.Abs(mouseDelta.x) !=0 )
        {
            transform.rotation *= Quaternion.Euler(0, 0.5f * mouseDelta.x, 0);
        }
        if (mouseDelta.y > 0 && (transform.rotation.eulerAngles.x < 90 || transform.rotation.eulerAngles.x > 315) 
            || mouseDelta.y < 0 && (transform.rotation.eulerAngles.x < 45 || transform.rotation.eulerAngles.x > 270))
        {
            
            transform.rotation *= Quaternion.Euler(0.5f * -mouseDelta.y, 0, 0);
        }
        transform.rotation = Quaternion.Euler(/*FPSCam.transform.rotation.eulerAngles.x*/0, transform.rotation.eulerAngles.y, 0);
        FPSCam.transform.rotation = Quaternion.Euler(FPSCam.transform.rotation.eulerAngles.x, FPSCam.transform.rotation.eulerAngles.y, 0);
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
                UiScript.instance.RemoveText();
                Destroy(actualMedikit);
            }
            else
            {
                UiScript.instance.FullInventory();
            }
        }
        else if (context.performed && IsAmmo)
        {
            if (BulletsInventory.instance.counter <= BulletsInventory.instance.maxCounter)
            {
                BulletsInventory.instance.AddInventory();
                UiScript.instance.RemoveText();
                Destroy(actualAmmo);
            }
            else
            {
                UiScript.instance.FullInventory();
            }
        }
        else if (context.performed && isDoor || isDoorLocked)
        {
            if (isDoorLocked)
            {
                isLockedAndUTry = true;
                UiScript.instance.RemoveText();
                UiScript.instance.DoorLockedMessage();
            }
            else
            {
                UiScript.instance.RemoveText();

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
        if (collision.gameObject.tag == "Bed")
        {
            isBed = true;
            UiScript.instance.InteractMessage();
            currentBed = collision.transform;
        }
        else if (collision.gameObject.tag == "Medikit")
        {
            isMedikit = true;
            UiScript.instance.InteractMessage();
            actualMedikit = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Ammo")
        {
            IsAmmo = true;
            UiScript.instance.InteractMessage();
            actualAmmo = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Door")
        {
            isDoorLocked = false;
            isDoor = true;
            UiScript.instance.InteractMessage();

            DoorAnim = collision.GetComponent<Animator>();
        }
        else if (collision.gameObject.tag == "LockedDoor")
        {
            isDoorLocked = true;
            UiScript.instance.InteractMessage();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bed"))
        {
            UiScript.instance.RemoveText();
            isBed = false;
        }
        else if (other.gameObject.CompareTag("Medikit"))
        {
            UiScript.instance.RemoveText();
            isMedikit = false;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            UiScript.instance.RemoveText();
            isDoor = false;
            isDoorLocked = false;

            DoorAnim.SetBool("Open", false);
            DoorAnim.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("LockedDoor"))
        {
            UiScript.instance.RemoveText();
            isDoor = false;
            isDoorLocked = false;
        }
        else if (other.gameObject.tag == "Ammo")
        {
            UiScript.instance.RemoveText();
            IsAmmo = false;
        }
    }
}
