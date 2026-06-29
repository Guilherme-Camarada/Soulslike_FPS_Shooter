using System;
using UnityEngine;

[Serializable]
public class IncreaseMoveSpeedEffect : UpgradeEffect
{
    [SerializeField] private float _amount;

    public override void Execute(UpgradeInventory upgradeInventory)
    {
        upgradeInventory.TryGetComponent(out PlayerMovement playerMovement);

        float currentMoveSpeed = playerMovement.GetCharacterSpeed();

        float newCharacterSpeed = currentMoveSpeed + _amount;

        playerMovement.SetCharacterSpeed(newCharacterSpeed);
    }

    public override void ExecuteContinuous(UpgradeInventory upgradeInventory)
    {
        
    }

    public override string GetDescription()
    {
        return $"Increase Character Speed by {_amount}";
    }
}
