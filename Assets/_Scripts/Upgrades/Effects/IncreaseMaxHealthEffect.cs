using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class IncreaseMaxHealthEffect : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out Damageable damageable);

        float newAmount = damageable.MaxHealth + _amount;

        damageable.SetMaxHealth(newAmount);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increase Max Health by {_amount}";
    }
}
