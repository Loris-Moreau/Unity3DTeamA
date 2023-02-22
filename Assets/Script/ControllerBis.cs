using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerBis : MonoBehaviour
{
    #region Movement & Player characteristic
    [Space]
    [Header("Movements")]
    [Space]

    public float speed = 5;
    private Vector2 direction;



    #endregion

    #region Ui
    [Space]
    [Header("Texte Medikit Inventaire Plein")]
    [Space]
    public GameObject textMedikitNonAvailable;
    public TextMeshProUGUI medikitFullText;
    public string txtFullMedikit;

    [Space]
    [Header("Texte Interaction")]
    [Space]
    public GameObject interactMessage;
    public TextMeshProUGUI interactionMsg;
    [TextArea]
    public string txtBed, txtMedikit, txtBullets;

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

    public int timeTextMedKit;
    public int heal = 30;

    #endregion

    #region Bullets
    [Space]
    [Header("Bullets")]
    [Space]
    public bool isBullet;
    public GameObject actualBullets;

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

    #endregion

    #region Animations
    [Space]
    [Header("Animation")]
    [Space]

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
        medikit = 1;

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
            respawnPoint = currentBed;
            fade.FadeIn();
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
                medikitFullText.text = txtFullMedikit;
                Invoke("RemoveText", timeTextMedKit);
            }
        }
        else if(context.performed && isBullet)
        {
            BulletsInventory.instance.AddInventory();
        }
    }

    public void Healing(InputAction.CallbackContext context)
    {
        if(context.performed && PlayerLife.instance.health < PlayerLife.instance.maxHealth && medikit > 0)
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
        else if(collision.gameObject.tag == "Medikit")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = txtMedikit;
            isMedikit = true;
            actualMedikit = collision.gameObject;
        }
        else if(collision.gameObject.tag == "Bullets")
        {
            interactMessage.SetActive(true);
            interactionMsg.text = txtBullets;
            isBullet = true;
            actualBullets= collision.gameObject;
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
            isMedikit = false;
        }
        else if(other.gameObject.tag == "Bullets")
        {
            interactMessage.SetActive(false);
            isBullet = false;
        }
    }

    void RemoveText()
    {
        textMedikitNonAvailable.SetActive(false);
    }
}
