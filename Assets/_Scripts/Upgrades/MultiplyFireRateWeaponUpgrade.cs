using NaughtyAttributes;
using UnityEngine;

public class MultiplyFireRateWeaponUpgrade : Upgrade
{
    [SerializeField] private float _multiplier = 1.5f;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out EquipParent equipParentInteractable);

        ShootUsable shootUsable = equipParentInteractable.EquipInteractable.GetComponent<ShootUsable>();

        float fireRate = shootUsable.FireRateCooldown;

        Debug.Log($"Old FireRate: {fireRate}");

        float newFireRate = fireRate - (fireRate * _multiplier);

        if (newFireRate < 0)
        {
            newFireRate = 0;
        }

        Debug.Log($"New FireRate: {newFireRate}");

        shootUsable.FireRateCooldown = newFireRate;
    }
}
