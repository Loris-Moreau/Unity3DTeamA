using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    #region Movement
    [Space]
    [Header("Movements")]
    [Space]

    public float speed = 5;
    private Vector2 direction;
    #endregion

    #region Medikit
    public bool isMedikit;

    public GameObject textMedikitNonAvailable;
    public GameObject actualMedikit;

    public int maxMedikit = 5;
    public int medikit;
    public int timeTextMedKit;
    public int heal = 30;

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

    public GameObject interactMessage;
    public TextMeshProUGUI interactionMsg;

    #endregion

    #region Animations
    [Space]
    [Header("Animation")]
    [Space]

    public Animation fadeOutSleep;
    public Animator animator;
    #endregion

    #region Mouse Look
    /*[Space]
    [Header("Mouse Follow")]
    [Space]

    public float rotationSpeed = 10.0f;
    //private Quaternion targetRotation;
    //private Quaternion currentRotation;
    */ 
    /*[Space]
    [Header("Rotation Settings \n")]
    [Space]

    private float pitch;
    [SerializeField][Range(-90.0f, 0)] public float angleClampY = -90f;
    [SerializeField][Range(0, 90.0f)] public float angleClampZ = 90f;*/
    #endregion

    private void Start()
    {
        medikit = 1;

        transform.position = respawnPoint.position;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        if(direction.y != 0) transform.position += speed * Time.deltaTime *  new Vector3(transform.forward.x, 0, transform.forward.z * direction.y);
        transform.position += speed * Time.deltaTime * transform.right * direction.x;//* new Vector3(direction.x, 0, direction.y);
    }

    public void UpdateRotation(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        Debug.Log("DELTA "+mouseDelta);
        Debug.Log("rot = "+transform.rotation.eulerAngles);

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

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed && isBed)
        {
            //fadeOutSleep.animation
            respawnPoint = currentBed;
        }
        else if (context.performed && isMedikit)
        {
            if (medikit < 5)
            {
                medikit++;
                Destroy(actualMedikit);
            }
            else
            {
                textMedikitNonAvailable.SetActive(true);
                Invoke("RemoveText", timeTextMedKit);
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
            isBed = true;
            currentBed = collision.transform;
        }
        else if (collision.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = "+1 Medikit";
            isMedikit = true;
            actualMedikit = collision.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Bed")
        {
            interactMessage.SetActive(false);
            isBed = false;
        }
        else if (other.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(false);
            isMedikit = false;

        }
    }
    void RemoveText()
    {
        textMedikitNonAvailable.SetActive(false);
    }
}
