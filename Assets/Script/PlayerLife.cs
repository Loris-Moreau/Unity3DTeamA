using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int heal;
    public GameObject[] bloodEffect;

    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealPlayer()
    {
        if (health < maxHealth)
        {
            health += heal;
        }
        if(health >= 75)
        {
            bloodEffect[0].SetActive(true);
        }
        else if(health >= 50 && health <= 74)
        {
            bloodEffect[0].SetActive(false);
            bloodEffect[1].SetActive(true);
        }
        else if(health >= 25 && health <= 49)
        {
            bloodEffect[1].SetActive(false);
            bloodEffect[2].SetActive(true);
        }
        else if(health >= 0 && health <= 24)
        {
            bloodEffect[2].SetActive(false);
            bloodEffect[3].SetActive(true);
        }
    }

    public void Hurt(int dmg)
    {
        health -= dmg;     
    }

    public void Death()
    {
        if(health <= 0)
        {

        }
    }
}
