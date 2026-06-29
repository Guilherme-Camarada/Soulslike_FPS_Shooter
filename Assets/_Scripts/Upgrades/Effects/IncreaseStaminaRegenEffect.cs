using System;
using UnityEngine;

[Serializable]
public class IncreaseStaminaRegenEffect : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentStaminaRegen = playerMovement.GetStaminaRegenRate();

        float newStaminaRegen = currentStaminaRegen + _amount;

        playerMovement.SetStaminaRegenRate(newStaminaRegen);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increase Stamina Regen Rate by {_amount}";
    }
}
