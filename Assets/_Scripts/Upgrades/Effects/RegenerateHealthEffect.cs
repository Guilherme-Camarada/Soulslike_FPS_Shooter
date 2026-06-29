using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class RegenerateHealthEffect : UpgradeEffect
{
    [SerializeField] private float _interval;
    [SerializeField] private float _amount;

    private Damageable _playerDamageable;
    private float _timer;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out Damageable damageable);

        _playerDamageable = damageable;

        _timer = 0f;
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        if (_playerDamageable == null) return;

        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            float newHealth = _playerDamageable.GetCurrentHealth() + _amount;
            _playerDamageable.SetCurrentHealth(newHealth);

            _timer = 0f;
        }
    }

    public override string GetDescription()
    {
        return $"Regenerates {_amount} health every {_interval} seconds.";
    }
}
