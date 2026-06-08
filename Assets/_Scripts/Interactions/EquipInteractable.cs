using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EquipInteractable : Interactable
{
    public event Action<bool> OnEquipAction;

    private bool _isEquipped;
    public bool IsEquipped
    {
        get => _isEquipped;
        set
        {
            if (_isEquipped != value)
            {
                _isEquipped = value;
                OnEquipAction?.Invoke(_isEquipped);
            }
        }
    }
    private EquipParent _equipParent;

    private bool _isAnimatingEquip;
    public bool IsAnimatingEquip => _isAnimatingEquip;

    private bool _isFloatingAnimationPlaying;

    public void SetEquipParent(EquipParent equipParent)
    {
        if (equipParent != null && equipParent.HasEquipInteractable())
        {
            Debug.LogError("EquipParent already has an EquipInteractable assigned.");
            return;
        }

        _equipParent = equipParent;

        if (equipParent != null)
        {
            _equipParent.SetEquipInteractable(this);
            IsEquipped = true;
            _isFloatingAnimationPlaying = false;

            transform.SetParent(equipParent.EquipTargetTransform, true);
            TryGetComponent(out Rigidbody rigidbody);
            rigidbody.isKinematic = true;

            _isAnimatingEquip = true;

            float equipDuration = 0.3f;

            transform.DOKill();

            Sequence equipSequence = DOTween.Sequence();

            equipSequence.Append(transform.DOLocalMove(Vector3.zero, equipDuration).SetEase(Ease.OutBack));
            equipSequence.Join(transform.DOLocalRotateQuaternion(Quaternion.identity, equipDuration).SetEase(Ease.OutQuad));
            equipSequence.OnComplete(() =>
            {
                _isAnimatingEquip = false;
            });
        } else
        {
            transform.SetParent(null);
            IsEquipped = false;

            transform.DOKill();
            _isFloatingAnimationPlaying = true;
        }
    }

    public override void Interact(Interactor playerInteractor)
    {
        if (playerInteractor.TryGetComponent(out EquipParent equipParent))
        {
            SetEquipParent(equipParent);
        }
        else
        {
            Debug.LogError("Interactor does not have an EquipParent component.");
        }
    }

    private void Update()
    {
        HandleUnequipedAnimation();
    }

    private void HandleUnequipedAnimation()
    {
        if (!_isAnimatingEquip && !IsEquipped && !_isFloatingAnimationPlaying)
        {
            _isFloatingAnimationPlaying = true;

            transform.DOMove(new Vector3(0, 0.2f, 0), 2f).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            transform.DORotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}