using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Damageable _playerDamageable;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInventory _playerInventory;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private TextMeshProUGUI _ammoText;

    [Header("Animation Settings")]
    [SerializeField] private float _healthTweenDuration = 0.4f;
    [SerializeField] private float _staminaTweenDuration = 0.2f;

    private Tween _healthTween;
    private Tween _staminaTween;

    private void Start()
    {
        _healthBarImage.fillAmount = _playerDamageable.CurrentHealth / _playerDamageable.MaxHealth;

        if (_playerInventory.TryGetComponent(out RangedWeapon rangedWeapon))
        {
            rangedWeapon.OnAmmoChangedAction += RangedWeapon_OnAmmoChangedAction;
            SetAmmoText(rangedWeapon);
        }
        else
        {
            _ammoText.text = "";
        }
    }

    private void OnEnable()
    {
        _playerDamageable.OnDamageTakenAction += PlayerDamageable_OnDamageTakenAction;
        _playerMovement.OnStaminaChangedAction += PlayerMovement_OnStaminaChangedAction;
        _playerInventory.OnCurrentEquippableChanged += PlayerInventory_OnCurrentEquippableChanged;
    }

    private void PlayerInventory_OnCurrentEquippableChanged(Equippable previousEquippable, Equippable currentEquippable)
    {
        if (previousEquippable.TryGetComponent(out RangedWeapon rangedWeapon1))
        {
            rangedWeapon1.OnAmmoChangedAction -= RangedWeapon_OnAmmoChangedAction;
        }
        if (currentEquippable.TryGetComponent(out RangedWeapon rangedWeapon2))
        {
            rangedWeapon2.OnAmmoChangedAction += RangedWeapon_OnAmmoChangedAction;
            SetAmmoText(rangedWeapon2);
        }
        else
        {
            _ammoText.text = "";
        }
    }

    private void RangedWeapon_OnAmmoChangedAction(int currentAmmo, int totalAmmo)
    {
        SetAmmoText(currentAmmo, totalAmmo);
    }

    private void PlayerMovement_OnStaminaChangedAction()
    {
        float targetFill = _playerMovement.CurrentStamina / _playerMovement.MaxStamina;
        _staminaTween?.Kill();
        _staminaTween = _staminaBarImage.DOFillAmount(targetFill, _staminaTweenDuration).SetEase(Ease.OutSine);
    }

    private void PlayerDamageable_OnDamageTakenAction()
    {
        float targetFill = _playerDamageable.CurrentHealth / _playerDamageable.MaxHealth;
        _healthTween?.Kill();
        _healthTween = _healthBarImage.DOFillAmount(targetFill, _healthTweenDuration).SetEase(Ease.OutCubic);
    }

    private void OnDisable()
    {
        _playerDamageable.OnDamageTakenAction -= PlayerDamageable_OnDamageTakenAction;
        _playerMovement.OnStaminaChangedAction -= PlayerMovement_OnStaminaChangedAction;
        _playerInventory.OnCurrentEquippableChanged -= PlayerInventory_OnCurrentEquippableChanged;

        _healthTween?.Kill();
        _staminaTween?.Kill();
    }

    private void SetAmmoText(RangedWeapon rangedWeapon)
    {
        _ammoText.text = $"{rangedWeapon.CurrentAmmo}/{rangedWeapon.TotalAmmo}";
    }

    private void SetAmmoText(int currentAmmo, int totalAmmo)
    {
        _ammoText.text = $"{currentAmmo}/{totalAmmo}";
    }
}