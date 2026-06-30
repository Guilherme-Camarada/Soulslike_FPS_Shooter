using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Damageable _playerDamageable;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private EquipParent _equipParent;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _staminaText;
    [SerializeField] private TextMeshProUGUI _ammoText;

    [Header("Animation Settings")]
    [SerializeField] private float _healthTweenDuration = 0.4f;
    [SerializeField] private float _staminaTweenDuration = 0.2f;

    private Tween _healthTween;
    private Tween _staminaTween;

    private void Start()
    {
        _healthBarImage.fillAmount = _playerDamageable.CurrentHealth / _playerDamageable.MaxHealth;

        _ammoText.text = "";
        _healthText.text = _playerDamageable.GetCurrentHealth().ToString("F0");
        _staminaText.text = _playerMovement.GetCurrentStamina().ToString("F0");
    }

    private void OnEnable()
    {
        _playerDamageable.OnDamageTakenAction += PlayerDamageable_OnDamageTakenAction;
        _playerMovement.OnStaminaChangedAction += PlayerMovement_OnStaminaChangedAction;
        _equipParent.OnEquipInteractableChangedAction += EquipParent_OnEquipInteractableChangedAction;
    }

    private void EquipParent_OnEquipInteractableChangedAction(EquipInteractable previousEquippable, EquipInteractable currentEquippable)
    {
        PlayerInventory_OnCurrentEquippableChanged(previousEquippable, currentEquippable);
    }

    private void PlayerInventory_OnCurrentEquippableChanged(EquipInteractable previousEquippable, EquipInteractable currentEquippable)
    {
        if (previousEquippable != null && previousEquippable.TryGetComponent(out ShootUsable rangedWeapon1))
        {
            rangedWeapon1.OnAmmoChangedAction -= RangedWeapon_OnAmmoChangedAction;
        }

        if (currentEquippable == null)
        {
            _ammoText.text = "";
            return;
        }

        if (currentEquippable.TryGetComponent(out ShootUsable rangedWeapon2))
        {
            rangedWeapon2.OnAmmoChangedAction += RangedWeapon_OnAmmoChangedAction;
            SetAmmoText(rangedWeapon2);
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
        _staminaText.text = _playerMovement.GetCurrentStamina().ToString("F1");
    }

    private void PlayerDamageable_OnDamageTakenAction()
    {
        float targetFill = _playerDamageable.CurrentHealth / _playerDamageable.MaxHealth;
        _healthTween?.Kill();
        _healthTween = _healthBarImage.DOFillAmount(targetFill, _healthTweenDuration).SetEase(Ease.OutCubic);
        _healthText.text = _playerDamageable.GetCurrentHealth().ToString("F1");
    }

    private void OnDisable()
    {
        _playerDamageable.OnDamageTakenAction -= PlayerDamageable_OnDamageTakenAction;
        _playerMovement.OnStaminaChangedAction -= PlayerMovement_OnStaminaChangedAction;
        _equipParent.OnEquipInteractableChangedAction -= EquipParent_OnEquipInteractableChangedAction;

        _healthTween?.Kill();
        _staminaTween?.Kill();
    }

    private void SetAmmoText(ShootUsable rangedWeapon)
    {
        _ammoText.text = $"{rangedWeapon.CurrentAmmo}/{rangedWeapon.TotalAmmo}";
    }

    private void SetAmmoText(int currentAmmo, int totalAmmo)
    {
        _ammoText.text = $"{currentAmmo}/{totalAmmo}";
    }
}