using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Interactor playerInteractor;

    [SerializeField] private List<Transform> _spawnPositionList;
    [SerializeField] private List<WaveInformation> _waveInformationList;

    [SerializeField] private bool _isWaveSpawning;
    private int _currentWave;


    private void Update()
    {
        if (_isWaveSpawning)
        {
            StartCoroutine(SpawnWaveCoroutine());
        }
        


    }

    private IEnumerator SpawnWaveCoroutine()
    {
        _isWaveSpawning = false;

        WaveInformation currentWaveInformation = _waveInformationList[_currentWave];
        List<EnemySpawnInfo> enemySpawnInfoList = currentWaveInformation.EnemySpawnInfoList;

        WaitForSeconds spawnDelay = new WaitForSeconds(0.5f);

        foreach (EnemySpawnInfo spawnInfo in enemySpawnInfoList)
        {
            int enemySpawnAmount = spawnInfo.Quantity;

            while (enemySpawnAmount > 0)
            {
                Instantiate(spawnInfo.EnemyPrefab, _spawnPositionList[Random.Range(0, _spawnPositionList.Count)].position, Quaternion.identity)
                .GetComponent<EnemyAI>().OnInit(playerInteractor);
                enemySpawnAmount--;

                yield return spawnDelay;
            }

            yield return new WaitForSeconds(spawnInfo.DelayBeforeNextSpawn);
        }

        _currentWave++;
    }


    public void SpawnNextWave()
    {
        _isWaveSpawning = !_isWaveSpawning;
    }
}


[Serializable]
public struct WaveInformation
{
    public List<EnemySpawnInfo> EnemySpawnInfoList;
}

[Serializable]
public struct EnemySpawnInfo
{
    public GameObject EnemyPrefab;
    public int Quantity;
    public float DelayBeforeNextSpawn;
}
