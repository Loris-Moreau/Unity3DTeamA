using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public GameObject[] bloodEffect;
    public FadeOutSleeping fade;

    public static PlayerLife instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerLife dans la sc�ne");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void HealPlayer(int heal)
    {
        if (health < maxHealth)
        {
            health = Mathf.Clamp(health + heal, 0, maxHealth);
            UiScript.instance.HealthInfo();
            UiScript.instance.MedikitInfo();
        }
        
    }

    public void Hurt(int dmg)
    {
        health -= dmg;
        if(health < maxHealth)
        {
            return;
        }
        else
        {
            health = 0;
        }
        
        UiScript.instance.HealthInfo();
        UiScript.instance.MedikitInfo();
        Death();
    }

    public void Death()
    {
        if(health <= 0)
        {
            //animation
            //respawn
            fade.FadeIn();
            transform.position = GetComponent<Controller>().respawnPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            int damages = other.GetComponent<ZombieHealth>().zombieData.damages;
            damages = Random.Range(damages-5, damages);
            Hurt(damages);
        }
    }
}
