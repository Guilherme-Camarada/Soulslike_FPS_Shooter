using System;
using UnityEngine;

[Serializable]
public class IncreaseDamageEffect : UpgradeEffect
{
    [SerializeField] private float _damageIncrease;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        ProjectileShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ProjectileShootUsable>();

        shootUsable.GetProjectilePrefab().TryGetComponent(out Projectile projectile);

        float newDamage = projectile.GetDamage() + _damageIncrease;

        projectile.SetDamage(newDamage);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increases projectile damage by {_damageIncrease}";
    }
}
