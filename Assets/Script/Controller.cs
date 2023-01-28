using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [Header("Mouse Follow")]
    public InputAction mousePosition;
    public float rotationSpeed = 100.0f;
    public float rotationSmoothSpeed = 0.2f;
    private Quaternion targetRotation;

    [Header("Movements")]
    public float speed = 5;
    private Vector2 direction;

    [Header("Spawn")]
    public Transform respawnPoint;
    private Transform currentBed;
    
    [Header("Bool")]
    public bool isBed = false;

    [Header("Interact")]
    public GameObject interactMessage;

    [Header("Animation")]
    public Animation fadeOutSleep;
    private void Start()
    {
        transform.position = respawnPoint.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(direction.x, 0, direction.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime * rotationSpeed);
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
        float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, angle, 0);
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

