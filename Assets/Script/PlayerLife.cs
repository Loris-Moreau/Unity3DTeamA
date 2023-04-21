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
            Debug.LogWarning("Il y a plus d'une instance de PlayerLife dans la scène");
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
        if(health <= 0)
        {
            health = 0;
            Death();
        }
        UiScript.instance.HealthInfo();
        UiScript.instance.MedikitInfo();
    }

    public void Death()
    {
        fade.FadeIn();
        Invoke("ReLife", 1f);
    }

    public void ReLife()
    {
        transform.position = GetComponent<Controller>().respawnPoint.position;
        health = maxHealth;
        UiScript.instance.HealthInfo();
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
