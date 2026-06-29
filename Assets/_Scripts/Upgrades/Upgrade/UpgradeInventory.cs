using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInventory : MonoBehaviour
{
    private List<UpgradeEffect> _upgradeList = new List<UpgradeEffect>();

    public void AddUpgradeEffect(UpgradeEffect upgradeEffect)
    {
        _upgradeList.Add(upgradeEffect);

        upgradeEffect.Execute(this);
    }

    public void Update()
    {
        foreach (UpgradeEffect upgradeEffect in _upgradeList)
        {
            upgradeEffect.ExecuteContinuous(this);
        }
    }
}
