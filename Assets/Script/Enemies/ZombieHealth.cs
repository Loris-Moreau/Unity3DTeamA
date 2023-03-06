using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            zombieData.health--;
            if(zombieData.health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
