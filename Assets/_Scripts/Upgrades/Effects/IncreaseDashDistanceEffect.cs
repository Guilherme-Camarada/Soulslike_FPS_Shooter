using System;
using UnityEngine;

[Serializable]
public class IncreaseDashDistanceEffect : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentDashDistance = playerMovement.GetDashDistance();

        float newDashDistance = currentDashDistance + _amount;

        playerMovement.SetDashDistance(newDashDistance);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increases the dash distance by {_amount}.";
    }
}
