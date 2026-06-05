using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    private List<WaveData> waves = new();

    [ReadOnly]
    [SerializeField]
    private bool isRunning;

    private Coroutine routine;

    [Button("Start Waves")]
    public void StartWaves()
    {
        if (isRunning) return;

        routine = StartCoroutine(RunWaves());
    }

    [Button("Stop Waves")]
    public void StopWaves()
    {
        if (routine != null)
            StopCoroutine(routine);

        isRunning = false;
    }

    private IEnumerator RunWaves()
    {
        isRunning = true;

        for (int i = 0; i < waves.Count; i++)
        {
            int waveNumber = i + 1;
            WaveData wave = waves[i];

            Debug.Log($"Wave {waveNumber} started");

            yield return StartCoroutine(RunWave(wave));

            Debug.Log($"Wave {waveNumber} ended");

            if (wave.GracePeriod > 0)
                yield return new WaitForSeconds(wave.GracePeriod);
        }

        Debug.Log("You won");

        isRunning = false;
    }

    private IEnumerator RunWave(WaveData wave)
    {
        float elapsed = 0f;

        while (elapsed < wave.WaveDuration)
        {
            SpawnEnemy(wave);

            yield return new WaitForSeconds(wave.SpawnCooldown);
            elapsed += wave.SpawnCooldown;
        }
    }

    private void SpawnEnemy(WaveData wave)
    {
        if (wave.Enemies.Count == 0 || wave.SpawnPoints.Count == 0)
            return;

        Transform spawnPoint =
            wave.SpawnPoints[Random.Range(0, wave.SpawnPoints.Count)];

        GameObject prefab = GetWeightedEnemy(wave.Enemies);

        if (prefab != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private GameObject GetWeightedEnemy(List<EnemySpawnEntry> enemies)
    {
        float roll = Random.Range(0f, 100f);
        float current = 0f;

        foreach (var e in enemies)
        {
            current += e.SpawnChance;

            if (roll <= current)
                return e.Prefab;
        }

        return enemies[^1].Prefab;
    }
}