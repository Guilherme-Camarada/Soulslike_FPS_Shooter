using DG.Tweening;
using NaughtyAttributes;
using PixPlays.ElementalVFX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public event Action OnWaveStartAction;
    public event Action OnWaveEndAction;

    [SerializeField] private Transform _enemiesChaseTarget;

    [SerializeField]
    private List<WaveData> _waves = new();

    [SerializeField] private List<Transform> _spawnPoints = new();
    private List<Damageable> _spawnedEnemies = new();

    [ReadOnly]
    [SerializeField]
    private bool _isRunning;

    [ReadOnly]
    [SerializeField]
    private int _currentWaveIndex = -1;

    private Coroutine _waveRoutine;


    // ─────────────────────────────────────────────
    // NEXT WAVE
    // ─────────────────────────────────────────────
    [Button("Start Next Wave")]
    public void StartNextWave()
    {
        _isRunning = true;

        _currentWaveIndex++;

        if (_currentWaveIndex >= _waves.Count)
        {
            Debug.Log("You won");
            _isRunning = false;
            return;
        }

        if (_waveRoutine != null)
            StopCoroutine(_waveRoutine);

        _waveRoutine = StartCoroutine(RunWaveSequence(_waves[_currentWaveIndex]));
        OnWaveStartAction?.Invoke();
    }

    // ─────────────────────────────────────────────
    // STOP
    // ─────────────────────────────────────────────
    [Button("Stop Waves")]
    public void StopWaves()
    {
        if (_waveRoutine != null)
            StopCoroutine(_waveRoutine);

        _isRunning = false;
        _currentWaveIndex = -1;
    }

    // ─────────────────────────────────────────────
    // WAVE RUNNER
    // ─────────────────────────────────────────────
    private IEnumerator RunWaveSequence(WaveData wave)
    {
        int waveNumber = _currentWaveIndex + 1;

        Debug.Log($"Wave {waveNumber} started");

        int deadEnemies = 0;
        int enemiesToSpawn = wave.WaveTotalSpawns;

        while (deadEnemies < wave.WaveEndKillCount)
        {
            if (_spawnedEnemies.Count < enemiesToSpawn)
            {
                Damageable damageable = SpawnEnemy(wave);

                void OnEnemyDeath(Damageable damageable)
                {
                    deadEnemies++;
                    _spawnedEnemies.Remove(damageable);
                    damageable.OnDeathAction -= OnEnemyDeath;
                }

                damageable.OnDeathAction += OnEnemyDeath;

                yield return new WaitForSeconds(wave.SpawnCooldown);
            } else
            {
                yield return null;
            }
        }

        OnWaveEndAction?.Invoke();

        Debug.Log($"Wave {waveNumber} ended");

        OnWaveFinished();
    }

    private void OnWaveFinished()
    {
        // If this was the last wave → win immediately
        if (_currentWaveIndex >= _waves.Count - 1)
        {
            Debug.Log("You won");
            _isRunning = false;
            return;
        }

        foreach (Damageable damageable in _spawnedEnemies)
        {
            if (damageable == null) continue;

            damageable.TryGetComponent(out EnemyAI enemyAI);
            enemyAI.enabled = false;
            damageable.TryGetComponent(out NavMeshAgent navMeshAgent);
            navMeshAgent.enabled = false;

            damageable.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear)
                .SetLink(damageable.gameObject).OnComplete(() =>
                {
                    Destroy(damageable.gameObject);
                });
        }

        _spawnedEnemies.Clear();

        // Otherwise wait for player input
        Debug.Log("Wave complete. Ready for next wave.");
    }

    // ─────────────────────────────────────────────
    // SPAWN LOGIC
    // ─────────────────────────────────────────────
    private Damageable SpawnEnemy(WaveData wave)
    {
        Damageable damageable = null;

        if (wave.Enemies.Count == 0 || _spawnPoints.Count == 0)
            return damageable;

        Transform spawnPoint =
            _spawnPoints[Random.Range(0, _spawnPoints.Count)];

        GameObject prefab = GetWeightedEnemy(wave.Enemies);

        if (prefab != null)
        {
            damageable = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Damageable>();
            damageable.TryGetComponent(out EnemyAI enemyAI);
            enemyAI.SetChaseTarget(_enemiesChaseTarget);

            _spawnedEnemies.Add(damageable);
        }

        return damageable;
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