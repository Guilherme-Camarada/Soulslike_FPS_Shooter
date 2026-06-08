using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInventory : MonoBehaviour
{
    private List<Upgrade> _upgradeList = new List<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        _upgradeList.Add(upgrade);
        upgrade.Execute(this);

        upgrade.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            Destroy(upgrade.gameObject);
        });
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        _upgradeList.Remove(upgrade);
    }
}
