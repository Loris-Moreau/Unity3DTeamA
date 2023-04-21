using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public ZombieData zombieData;
    public int zombieHealth;

    public void Start()
    {
        zombieHealth = zombieData.health;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            zombieHealth--;
            if(zombieHealth <= 0)
            {
                Destroy(this);
            }
        }
    }
}
