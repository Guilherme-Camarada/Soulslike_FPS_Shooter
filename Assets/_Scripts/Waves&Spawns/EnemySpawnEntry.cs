using System;
using UnityEngine;

[Serializable]
public class EnemySpawnEntry
{
    public GameObject Prefab;

    [Range(0f, 100f)]
    public float SpawnChance;
}