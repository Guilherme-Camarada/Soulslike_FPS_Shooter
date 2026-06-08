using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class WaveData
{
    [Min(1)]
    public int WaveEndKillCount = 20;

    [Min(1)]
    public int WaveTotalSpawns = 30;

    [Min(0.1f)]
    public float SpawnCooldown = 1f;

    [BoxGroup("Enemies")]
    public List<EnemySpawnEntry> Enemies = new();
}