using System;
using UnityEngine;

[Serializable]
public class ChangeProjectileEffect : UpgradeEffect
{
    [SerializeField] private GameObject _newProjectilePrefab;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        ProjectileShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ProjectileShootUsable>();

        shootUsable.SetProjectilePrefab(_newProjectilePrefab);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Changes the projectile of the weapon to {_newProjectilePrefab.name}.";
    }
}
