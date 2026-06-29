using System;
using UnityEngine;

[Serializable]
public class IncreaseStaminaEffect : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentStamina = playerMovement.GetMaxStamina();

        float newStamina = currentStamina + _amount;

        playerMovement.SetMaxStamina(newStamina);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increase Max Stamina by {_amount}";
    }
}
