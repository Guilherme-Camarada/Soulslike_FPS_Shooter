using System;
using UnityEngine;

[Serializable]
public class SwitchToAutoEffect : UpgradeEffect
{
    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        ShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ShootUsable>();

        shootUsable.WeaponFireMode = FireMode.Automatic;
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {

    }

    public override string GetDescription()
    {
        return $"Makes the weapon fire in automatic mode.";
    }
}
