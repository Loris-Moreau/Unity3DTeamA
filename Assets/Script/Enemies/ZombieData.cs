using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    public float speed = 1f;
    public float detectionDistance = 3f;
    public float detectionStopDistance = 5f;
}
