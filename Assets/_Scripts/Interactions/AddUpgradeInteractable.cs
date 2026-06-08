using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddUpgradeInteractable : Interactable
{
    public event Action OnUpgradeAddedAction;

    private List<Upgrade> _upgradeList = new List<Upgrade>();

    private void Awake()
    {
        GetComponents(_upgradeList);
    }

    public override void Interact(Interactor playerInteractor)
    {
        playerInteractor.TryGetComponent(out UpgradeInventory upgradeInventory);

        foreach (Upgrade upgrade in _upgradeList)
        {
            upgradeInventory.AddUpgrade(upgrade);
        }

        OnUpgradeAddedAction?.Invoke();
    }
}
