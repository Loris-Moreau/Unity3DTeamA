using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public GameObject[] bloodEffect;

    public static PlayerLife instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerLife dans la scène");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void HealPlayer(int heal)
    {
        if (health < maxHealth)
        {
            health = Mathf.Clamp(health + heal, 0, maxHealth);
        }
    }

    public void Hurt(int dmg)
    {
        health -= dmg;
        ChangeHealthStats();
    }

    public void Death()
    {
        if(health <= 0)
        {
            //animation
            //respawn
            transform.position = GetComponent<Controller>().respawnPoint.position;
        }
    }

    public void ChangeHealthStats()
    {
        if (health >= 75 && health <= 99)
        {
            bloodEffect[0].SetActive(true);
        }
        else if (health >= 50 && health <= 74)
        {
            bloodEffect[0].SetActive(false);
            bloodEffect[1].SetActive(true);
        }
        else if (health >= 25 && health <= 49)
        {
            bloodEffect[1].SetActive(false);
            bloodEffect[2].SetActive(true);
        }
        else if (health >= 0 && health <= 24)
        {
            bloodEffect[2].SetActive(false);
            bloodEffect[3].SetActive(true);
        }
    }
}
