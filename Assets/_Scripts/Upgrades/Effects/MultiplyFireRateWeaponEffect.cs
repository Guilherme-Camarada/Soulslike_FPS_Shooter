using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
public class MultiplyFireRateWeaponEffect : UpgradeEffect
{
    [SerializeField] private float _multiplier = 1.5f;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);
            
        ShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ShootUsable>();

        float fireRate = shootUsable.FireRateCooldown;

        float newFireRate = fireRate - (fireRate * _multiplier);

        if (newFireRate < 0)
        {
            newFireRate = 0;
        }

        shootUsable.FireRateCooldown = newFireRate;
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Multiplies the fire rate of the weapon by {_multiplier * 100}%.";
    }
}
