using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStartedAction;

    [SerializeField] private List<Transform> _weaponSpawnPositions;
    [SerializeField] private List<ShootUsable> _shootUsableList;

    private List<EquipInteractable> _spawnedInteractables = new List<EquipInteractable>();

    private bool _hasGameStarted;

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
        SpawnRandomWeapons(_weaponSpawnPositions.Count, _shootUsableList);
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

            Transform spawnPosition = _weaponSpawnPositions[i];
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
            _hasGameStarted = true;
            OnGameStartedAction?.Invoke();
            Debug.Log("Game Started!");

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
