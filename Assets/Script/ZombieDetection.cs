using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class ZombieDetection : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Transform player;
    public float detectionDistance = 3f;
    public float zombieSpeed = 1f;
    private Vector3 rotVelocity;
    public float smoothRotation = 0.05f;

    private void FixedUpdate()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();
        float dot = Vector3.Dot(dir, transform.forward);

        if (dot > 0.8f && Vector3.Distance(transform.position, player.position) < detectionDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, zombieSpeed * Time.fixedDeltaTime);
        }
        dir.y = 0f;
        transform.forward = Vector3.SmoothDamp(transform.forward, dir, ref rotVelocity, smoothRotation);
    }
}
