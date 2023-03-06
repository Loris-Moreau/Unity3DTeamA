using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private int health;

    private void Start()
    {
        health = zombieData.health;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            health--;
            if(health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
