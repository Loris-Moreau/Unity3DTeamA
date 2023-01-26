using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform rocketLauncher;

    private float xRotationCamera = 0f;
    private float xRotationRocketLauncher = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotationCamera -= mouseY;
        xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

        xRotationRocketLauncher -= mouseY;
        xRotationRocketLauncher = Mathf.Clamp(xRotationRocketLauncher, 0f, 180f);

        transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        rocketLauncher.localRotation = Quaternion.Euler(xRotationRocketLauncher, 0f, 0f);
    }
}