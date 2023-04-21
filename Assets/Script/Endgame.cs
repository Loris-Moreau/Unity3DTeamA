using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    [SerializeField] private FadeOutSleeping fadeOutSleeping;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fadeOutSleeping.timeBetweenFade = Mathf.Infinity;
            fadeOutSleeping.FadeIn();
        }
    }
}
