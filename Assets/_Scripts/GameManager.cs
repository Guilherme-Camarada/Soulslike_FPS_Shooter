using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStartedAction;
    public event Action<GameState> OnStateChangedAction;

    [SerializeField] private WaveSpawner _waveSpawner;

    [SerializeField] private List<Transform> _pedestalSpawnPositions;
    [SerializeField] private List<ShootUsable> _shootUsableList;
    [SerializeField] private List<Upgrade> _upgradeList;

    private List<EquipInteractable> _spawnedInteractables = new List<EquipInteractable>();
    private List<AddUpgradeInteractable> _spawnedUpgrades = new();

    private bool _hasGameStarted;

    private GameState _gameState; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnRandomWeapons(_pedestalSpawnPositions.Count, _shootUsableList);
    }

    private void OnEnable()
    {
        _waveSpawner.OnWaveEndAction += WaveSpawner_OnWaveEndAction;
        _waveSpawner.OnLastWaveEndAction += WaveSpawner_OnLastWaveEndAction;
    }

    private void WaveSpawner_OnLastWaveEndAction()
    {
        HandleStateChange(GameState.GameEnded);
    }

    private void WaveSpawner_OnWaveEndAction()
    {
        HandleStateChange(GameState.ChoosingUpgrade);
    }

    private void OnDisable()
    {
        _waveSpawner.OnWaveEndAction -= WaveSpawner_OnWaveEndAction;
    }

    private void HandleStateChange(GameState newState)
    {
        if (newState == GameState.ChoosingUpgrade)
        {
            SpawnRandomUpgrades(_pedestalSpawnPositions.Count, _upgradeList);
        }
        else if (newState == GameState.WaveSpawning)
        {
            _waveSpawner.StartNextWave();
        } else if (newState == GameState.GameEnded)
        {
            Debug.Log("Game Ended");
        }

        _gameState = newState;

        OnStateChangedAction?.Invoke(_gameState);
    }

    private void SpawnRandomUpgrades(int amount, List<Upgrade> upgradeList)
    {
        List<Upgrade> availableUpgrades = new List<Upgrade>(upgradeList);

        for (int i = 0; i < amount; i++)
        {
            if (availableUpgrades.Count == 0)
            {
                Debug.LogWarning("Not enough unique upgrades to spawn.");
                break;
            }

            int randomIndex = Random.Range(0, availableUpgrades.Count);
            Upgrade selectedUpgrade = availableUpgrades[randomIndex];
            availableUpgrades.RemoveAt(randomIndex);

            Transform spawnPosition = _pedestalSpawnPositions[i];
            Upgrade instantiatedUpgrade = Instantiate(selectedUpgrade, spawnPosition.position, spawnPosition.rotation);

            AddUpgradeInteractable interactable = instantiatedUpgrade.GetComponent<AddUpgradeInteractable>();
            interactable.OnUpgradeAddedAction += Interactable_OnUpgradeAddedAction;

            _spawnedUpgrades.Add(interactable);
        }
    }

    private void Interactable_OnUpgradeAddedAction()
    {
        HandleStateChange(GameState.WaveSpawning);

        foreach (AddUpgradeInteractable interactable in _spawnedUpgrades)
        {
            interactable.IsInteractable = false;

            if (interactable != null)
            {
                interactable.OnUpgradeAddedAction -= Interactable_OnUpgradeAddedAction;
            }

            Sequence destroySequence = DOTween.Sequence();

            destroySequence.Append(interactable.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce))
                           .OnComplete(() => Destroy(interactable.gameObject));
        }

        _spawnedUpgrades.Clear();
    }

    private void SpawnRandomWeapons(int amount, List<ShootUsable> shootUsableList)
    {
        List<ShootUsable> availableWeapons = new List<ShootUsable>(shootUsableList);

        for (int i = 0; i < amount; i++)
        {
            if (availableWeapons.Count == 0)
            {
                Debug.LogWarning("Not enough unique weapons to spawn.");
                break;
            }

            int randomIndex = Random.Range(0, availableWeapons.Count);
            ShootUsable selectedWeapon = availableWeapons[randomIndex];
            availableWeapons.RemoveAt(randomIndex);

            Transform spawnPosition = _pedestalSpawnPositions[i];
            ShootUsable instantiatedWeapon = Instantiate(selectedWeapon, spawnPosition.position, spawnPosition.rotation);

            EquipInteractable equipInteractable = instantiatedWeapon.GetComponent<EquipInteractable>();
            equipInteractable.OnEquipAction += EquipInteractable_OnEquipAction;

            _spawnedInteractables.Add(equipInteractable);
        }
    }



    private void EquipInteractable_OnEquipAction(bool value)
    {
        if (value && !_hasGameStarted)
        {
            HandleStateChange(GameState.WaveSpawning);
            _hasGameStarted = true;
            OnGameStartedAction?.Invoke();

            foreach (EquipInteractable interactable in _spawnedInteractables)
            {
                if (interactable.IsEquipped) continue;
                
                interactable.IsInteractable = false;

                if (interactable != null)
                {
                    interactable.OnEquipAction -= EquipInteractable_OnEquipAction;
                }

                Sequence destroySequence = DOTween.Sequence();

                destroySequence.Append(interactable.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce))
                               .OnComplete(() => Destroy(interactable.gameObject));
            }     

            _spawnedInteractables.RemoveAll(interactable => interactable.IsEquipped == false);
        }
    }
}

public enum GameState
{
    ChoosingUpgrade,
    WaveSpawning,
    GameEnded
}
