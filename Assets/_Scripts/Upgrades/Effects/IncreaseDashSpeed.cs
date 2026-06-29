using System;
using UnityEngine;

[Serializable]
public class IncreaseDashSpeed : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentDashSpeed = playerMovement.GetDashSpeed();

        float newDashSpeed = currentDashSpeed + _amount;

        playerMovement.SetDashSpeed(newDashSpeed);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increases the dash speed by {_amount}.";
    }
}
