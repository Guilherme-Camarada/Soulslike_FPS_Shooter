using System;
using UnityEngine;

[Serializable]
public class DecreaseDashCostEffect : UpgradeEffect
{
    [SerializeField] private float _multiplier;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentStaminaCost = playerMovement.GetStaminaDashCost();

        float newStaminaCost = currentStaminaCost - (currentStaminaCost * _multiplier);

        if (newStaminaCost < 0)
        {
            newStaminaCost = 0;
        }

        playerMovement.SetStaminaDashCost(newStaminaCost);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Reduces the dash cost by {_multiplier * 100}%.";
    }
}
