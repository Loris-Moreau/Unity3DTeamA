using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class ZombieDetection : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private Rigidbody rb;

    #region Detection

    [SerializeField] private Transform playerEye;
    [SerializeField] private float smoothRotation = 0.05f;
    private bool playerDetected;
    private Vector3 rotVelocity;
    private Transform player;

    #endregion

    #region Wall Detection

    [Space]
    [Header("Collision Settings \n-------------------------")]
    [Space]

    [SerializeField] private Transform eye;
    [SerializeField] private bool viewObstacleTest = true;
    [SerializeField] private LayerMask collisionLayerMask = ~0;
    private RaycastHit hit;

    #endregion

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = Controller.Instance.transform;
    }

    private void FixedUpdate()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();
        float dot = Vector3.Dot(dir, transform.forward);

        if (!playerDetected)
        {
            if (dot > 0.8f && Vector3.Distance(transform.position, player.position) < zombieData.detectionDistance)
            {
                if(viewObstacleTest)
                {
                    if(CheckViewObstacles()) playerDetected = true;
                }
                else playerDetected = true;
            }
        }
        if (playerDetected)
        {
            if (Vector3.Distance(transform.position, player.position) < zombieData.detectionStopDistance)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, player.position, zombieData.speed * Time.fixedDeltaTime));
                dir.y = 0f;
                transform.forward = Vector3.SmoothDamp(transform.forward, dir, ref rotVelocity, smoothRotation);
            }
            else playerDetected = false;
        }
    }

    private bool CheckViewObstacles()
    {
        Vector3 dir = playerEye.position - eye.position;
        dir.Normalize();
        Physics.Raycast(eye.position, dir, out hit, zombieData.detectionDistance, collisionLayerMask);
        return (hit.collider.gameObject.CompareTag("Player"));
    }
}
