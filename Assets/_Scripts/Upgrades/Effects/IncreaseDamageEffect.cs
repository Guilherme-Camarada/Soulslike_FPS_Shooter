using System;
using UnityEngine;

[Serializable]
public class IncreaseDamageEffect : UpgradeEffect
{
    [SerializeField] private float _damageIncrease;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        if (equipParentInteractable.EquipInteractable.TryGetComponent(out ProjectileShootUsable shootUsable))
        {
            shootUsable.SetBonusDamage(shootUsable.GetBonusDamage() + _damageIncrease);
        }
        else if (equipParentInteractable.EquipInteractable.TryGetComponent(out ProjectileScatterShootUsable scatterUsable))
        {
            scatterUsable.SetBonusDamage(scatterUsable.GetBonusDamage() + _damageIncrease);
        }
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increases projectile damage by {_damageIncrease}";
    }
}
