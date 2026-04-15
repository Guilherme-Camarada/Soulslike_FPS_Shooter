using System;
using UnityEngine;

public class WaveStartInteractable : Interactable
{
    [SerializeField] private EnemySpawner _enemySpawner;



    public override void Interact(Interactable interactable, PlayerInteractor playerInteractor)
    {
        _enemySpawner.SpawnNextWave();
    }

    public override void InteractSecondary(Interactable interactable, PlayerInteractor playerInteractor)
    {
        throw new System.NotImplementedException();
    }
}
