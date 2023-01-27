using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform Gun;

    private float xRotationCamera = 0f;
    private float xRotationGun = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
    //}

    /*public void MouseMouvement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {*/
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotationCamera -= mouseY;
            xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

            xRotationGun -= mouseY;
            xRotationGun = Mathf.Clamp(xRotationGun, 0f, 180f);
            
            transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
            Gun.localRotation = Quaternion.Euler(xRotationGun, 0f, 0f);
        //}
    }
}