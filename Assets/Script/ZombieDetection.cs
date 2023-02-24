using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class ZombieDetection : MonoBehaviour
{
    [Header("Detection Settings \n-------------------------")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float zombieSpeed = 1f;
    [SerializeField] private float smoothRotation = 0.05f;
    [Space]
    [SerializeField] private float detectionDistance = 3f;
    [SerializeField] private float detectionStopDistance = 5f;
    private bool playerDetected;
    private Vector3 rotVelocity;
    private Transform player;

    #region Wall Detection

    [Space]
    [Header("Collision Settings \n-------------------------")]
    [Space]
    [SerializeField] private Transform eye;
    [SerializeField] private bool doCollisionTest = true;
    [SerializeField] private float collisionSphereSize = 0.1f;
    [SerializeField] private LayerMask collisionLayerMask = ~0;

    private RaycastHit hit;
    private Vector3[] raycastPositions;

    #endregion

    #region Debug

    [Space]
    [Header("Debugging \n-------------------------")]
    [Space]

    [SerializeField] private bool visualDebugging = true;
    [SerializeField] private Color sphereColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);

    #endregion

    private void Start()
    {
        player = Controller.Instance.transform;
    }

    private void FixedUpdate()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();
        float dot = Vector3.Dot(dir, transform.forward);

        if (!playerDetected)
        {
            if (dot > 0.8f && Vector3.Distance(transform.position, player.position) < detectionDistance)
            {
                playerDetected = true;
            }
        }
        if (playerDetected)
        {
            //CheckCollisions();

            if (Vector3.Distance(transform.position, player.position) < detectionStopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, zombieSpeed * Time.fixedDeltaTime);
                dir.y = 0f;
                transform.forward = Vector3.SmoothDamp(transform.forward, dir, ref rotVelocity, smoothRotation);
            }
            else playerDetected = false;
        }
    }

    private void CheckCollisions()
    {
        if (doCollisionTest)
        {
            Vector3 dir = player.position - eye.position;
            Physics.SphereCast(eye.position, collisionSphereSize, dir, out hit, detectionStopDistance, collisionLayerMask);
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                playerDetected = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!visualDebugging)
            return;

        Handles.color = sphereColor;
    }
}
