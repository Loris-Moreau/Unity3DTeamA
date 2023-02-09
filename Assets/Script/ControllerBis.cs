using TMPro;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerBis : MonoBehaviour
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
    private TextMeshProUGUI interactionMsg;
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

    public InputAction mousePosition;
    public float rotationSpeed = 10.0f;
    private Quaternion targetRotation;
    private Quaternion currentRotation;
    public float rotationThreshold = 0.1f;
    Vector2 mousePos;
    #endregion

    private void Start()
    {
        transform.position = respawnPoint.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(direction.x, 0, direction.y);

        currentRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        Debug.Log("Current");
        Debug.Log(currentRotation);
        Debug.Log("Taget");
        Debug.Log(targetRotation);
    }

    private void OnEnable()
    {
        mousePosition.performed += UpdateRotation;
    }

    private void OnDisable()
    {
        mousePosition.performed -= UpdateRotation;
    }

    private void UpdateRotation(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        float angle = -Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, angle, 0);

        if(Quaternion.Angle(currentRotation, targetRotation) < rotationThreshold)
        {
            currentRotation = targetRotation;
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
            interactionMsg.text = "Une bonne nuit de sommeil s'impose...";
            isBed = true;
            currentBed = collision.transform;
        }
        else if(collision.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = "Heal (1)";

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Bed")
        {
            interactMessage.SetActive(false);
            isBed = false;
        }
        else if(other.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(false);
        }
    }

    
}
