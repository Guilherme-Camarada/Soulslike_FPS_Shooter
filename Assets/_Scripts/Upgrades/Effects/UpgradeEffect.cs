using System;
using UnityEngine;

[Serializable]
public abstract class UpgradeEffect
{
    public abstract void Execute(UpgradeInventory upgradeInventory);
    public abstract void ExecuteContinuous(UpgradeInventory upgradeInventory);

    public abstract string GetDescription();
}
