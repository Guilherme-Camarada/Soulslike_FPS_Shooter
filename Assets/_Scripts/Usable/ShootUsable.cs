using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EquipInteractable))]
public abstract class ShootUsable : Usable
{
    private EquipInteractable _equipInteractable;

    public event Action OnFireAction;
    public event Action <float> OnReloadAction;
    public event Action <int, int> OnAmmoChangedAction;

    [Header("Weapon Settings")]
    [SerializeField] private FireMode _weaponFireMode;
    public FireMode WeaponFireMode { get => _weaponFireMode; set => _weaponFireMode = value; }

    [SerializeField] private float _fireRateCooldown = 0.4f;    
    public float FireRateCooldown { get => _fireRateCooldown; set => _fireRateCooldown = value; }
    [SerializeField] private float _reloadCooldown = 2f;
    public float ReloadCooldown { get => _reloadCooldown; set => _reloadCooldown = value; }
    [SerializeField] private int _totalAmmo = 15;

    [SerializeField] protected Transform _shootOrigin;
    public Transform ShootOrigin { get => _shootOrigin; set => _shootOrigin = value; }

    private bool _isReloading;
    private int _currentAmmo;

    public int CurrentAmmo => _currentAmmo;
    public int TotalAmmo
    {
        get => _totalAmmo;
        set
        {
            if (_totalAmmo == value) return;

            _totalAmmo = value;
            OnAmmoChangedAction?.Invoke(CurrentAmmo, _totalAmmo);
        }
    }

    private float _fireCooldownTimer = 0f;
    private float _reloadCooldownTimer = 0f;
     
    private Coroutine _shootingCoroutine;

    private void Awake()
    {
        _equipInteractable = GetComponent<EquipInteractable>();

        _currentAmmo = _totalAmmo;
        OnAmmoChangedAction?.Invoke(_currentAmmo, _totalAmmo);
    }

    protected virtual void Update()
    {
        if (_fireCooldownTimer >= 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;
        }
        if (_reloadCooldownTimer >= 0f)
        {
            _reloadCooldownTimer -= Time.deltaTime;

            if (_reloadCooldownTimer <= 0f && _isReloading)
            {
                _currentAmmo = _totalAmmo;
                OnAmmoChangedAction?.Invoke(_currentAmmo, _totalAmmo);
                _isReloading = false;
            }
        }
    }

    private void OnEnable()
    {
        _equipInteractable.OnEquipAction += EquipInteractable_OnEquipAction;
    }

    private void EquipInteractable_OnEquipAction(bool isEquipped)
    {
        if (!isEquipped)
        {
            _isReloading = false;
            _reloadCooldownTimer = 0f;

            StopAttack();
        } 
    }

    private void OnDisable()
    {
        _equipInteractable.OnEquipAction -= EquipInteractable_OnEquipAction;
    }

    public virtual bool TryShoot()
    {
        if (CanFire())
        {
            OnFireAction?.Invoke();

            _currentAmmo--;
            OnAmmoChangedAction?.Invoke(_currentAmmo, _totalAmmo);

            _fireCooldownTimer = _fireRateCooldown;
            return true;
        }
        else if (_currentAmmo <= 0)
        {
            Reload();
            return false;
        }

        return false;
    }

    public void Reload()
    {
        if (_isReloading || _currentAmmo == _totalAmmo)
        {
            return;
        }

        _isReloading = true;
        _reloadCooldownTimer = _reloadCooldown;
        OnReloadAction?.Invoke(_reloadCooldownTimer);
    }

    public bool CanFire()
    {
        if (_fireCooldownTimer <= 0f && _currentAmmo > 0 && !_isReloading && _reloadCooldownTimer <= 0f)
        {
            return true;
        }

        return false;
    }

    public override void UseStart()
    {
        base.UseStart();
        Attack();
    }

    public override void UseStop()
    {
        base.UseStop();
        StopAttack();
    }

    public void Attack()
    {
        if (WeaponFireMode == FireMode.Semi_Automatic)
        {
            TryShoot();
        }
        else if (WeaponFireMode == FireMode.Automatic)
        {
            if (_shootingCoroutine == null)
            {
                _shootingCoroutine = StartCoroutine(AutoShootRoutine());
            }
        }
    }

    public void StopAttack()
    {
        if (_shootingCoroutine != null)
        {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }

    private IEnumerator AutoShootRoutine()
    {
        while (true)
        {
            TryShoot();

            yield return null;
        }
    }
}

public enum FireMode
{
    Automatic,
    Semi_Automatic
}