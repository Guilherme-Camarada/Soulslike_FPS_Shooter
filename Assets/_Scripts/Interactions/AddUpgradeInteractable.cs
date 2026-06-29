using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddUpgradeInteractable : Interactable
{
    public event Action OnUpgradeAddedAction;
    private Upgrade _upgrade;

    private void Awake()
    {
        _upgrade = GetComponent<Upgrade>();
    }

    public override void Interact(Interactor playerInteractor)
    {
        playerInteractor.TryGetComponent(out UpgradeInventory upgradeInventory);

        foreach (UpgradeEffect upgradeEffect in _upgrade.GetUpgradeEffectList())
        {
            upgradeInventory.AddUpgradeEffect(upgradeEffect);
        }

        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
