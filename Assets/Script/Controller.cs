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
    #endregion

    #region Animations
    [Space]
    [Header("Animation")]
    [Space]

    public Animation fadeOutSleep;
    public Animator animator;
    #endregion

    #region Mouse Look
    [Space]
    [Header("Mouse Follow")]
    [Space]

    public float rotationSpeed = 10.0f;
    //private Quaternion targetRotation;
    //private Quaternion currentRotation;

    /*[Space]
    [Header("Rotation Settings \n")]
    [Space]

    private float pitch;
    [SerializeField][Range(-90.0f, 0)] public float angleClampY = -90f;
    [SerializeField][Range(0, 90.0f)] public float angleClampZ = 90f;*/
    #endregion

    private void Start()
    {
        transform.position = respawnPoint.position;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(direction.x, 0, direction.y);

        //currentRotation = transform.rotation;
        //transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void UpdateRotation(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        Debug.Log(mouseDelta);

        if (Mathf.Abs(mouseDelta.x) !=0)
        {
            transform.Rotate(0, 0.5f * mouseDelta.x, 0);
        }
        if (Mathf.Abs(mouseDelta.y) !=0)
        {
            transform.Rotate(0.5f * mouseDelta.y, 0, 0);
        }
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
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Bed")
        {
            interactMessage.SetActive(true);
            isBed = true;
            currentBed = collision.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Bed")
        {
            interactMessage.SetActive(false);
            isBed = false;
        }
    }
}
