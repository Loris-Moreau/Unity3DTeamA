using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    public int health = 2;
    public int damages = 1;
    public float speed = 1f;
    public float detectionDistance = 3f;
    public float detectionStopDistance = 5f;
}
