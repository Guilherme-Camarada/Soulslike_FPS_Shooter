using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class WaveData
{
    [Min(0.1f)]
    public float WaveDuration = 20f;

    [Min(0.1f)]
    public float SpawnCooldown = 1f;

    [Min(0f)]
    public float GracePeriod = 5f;

    [BoxGroup("Enemies")]
    public List<EnemySpawnEntry> Enemies = new();

    [BoxGroup("Spawn Points")]
    public List<Transform> SpawnPoints = new();
}