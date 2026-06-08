using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public abstract void Execute(UpgradeInventory upgradeInventory);
}
