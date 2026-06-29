using System;
using UnityEngine;

[Serializable]
public class AmmoIncreaseEffect : UpgradeEffect
{
    [SerializeField] private int _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        ShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ShootUsable>();

        shootUsable.TotalAmmo += _amount;
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increases the total ammo of the weapon by {_amount}.";
    }
}
