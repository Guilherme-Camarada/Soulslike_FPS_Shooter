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

    [ReadOnly]
    [SerializeField]
    private int currentWaveIndex = -1;

    private Coroutine waveRoutine;

    // ─────────────────────────────────────────────
    // START RUN
    // ─────────────────────────────────────────────
    [Button("Start Waves")]
    public void StartWaves()
    {
        if (isRunning) return;

        currentWaveIndex = -1;
        isRunning = true;

        StartNextWave();
    }

    // ─────────────────────────────────────────────
    // NEXT WAVE
    // ─────────────────────────────────────────────
    [Button("Start Next Wave")]
    public void StartNextWave()
    {
        if (!isRunning)
            return;

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("You won");
            isRunning = false;
            return;
        }

        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(RunWaveSequence(waves[currentWaveIndex]));
    }

    // ─────────────────────────────────────────────
    // STOP
    // ─────────────────────────────────────────────
    [Button("Stop Waves")]
    public void StopWaves()
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        isRunning = false;
        currentWaveIndex = -1;
    }

    // ─────────────────────────────────────────────
    // WAVE RUNNER
    // ─────────────────────────────────────────────
    private IEnumerator RunWaveSequence(WaveData wave)
    {
        int waveNumber = currentWaveIndex + 1;

        Debug.Log($"Wave {waveNumber} started");

        float elapsed = 0f;

        while (elapsed < wave.WaveDuration)
        {
            SpawnEnemy(wave);

            yield return new WaitForSeconds(wave.SpawnCooldown);
            elapsed += wave.SpawnCooldown;
        }

        Debug.Log($"Wave {waveNumber} ended");

        OnWaveFinished();
    }

    private void OnWaveFinished()
    {
        // If this was the last wave → win immediately
        if (currentWaveIndex >= waves.Count - 1)
        {
            Debug.Log("You won");
            isRunning = false;
            return;
        }

        // Otherwise wait for player input
        Debug.Log("Wave complete. Ready for next wave.");
    }

    // ─────────────────────────────────────────────
    // SPAWN LOGIC
    // ─────────────────────────────────────────────
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