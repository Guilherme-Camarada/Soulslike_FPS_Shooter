using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class RangedWeapon : Equippable
{
    public event Action OnFireAction;
    public event Action <float> OnReloadAction;
    public event Action <int, int> OnAmmoChangedAction;    

    [Header("Weapon Settings")]
    [SerializeField] private FireMode _weaponFireMode;
    public FireMode WeaponFireMode => _weaponFireMode;

    [SerializeField] private float _fireRateCooldown = 0.4f;
    [SerializeField] private float _reloadCooldown = 2f;
    [SerializeField] private int _totalAmmo = 15;

    [Header("Recoil Settings")]
    [SerializeField] private MouseLook _mouseLook;
    [Range(0f, 7f)][SerializeField] private float _recoilAmountY = 3f;
    [Range(0f, 3f)][SerializeField] private float _recoilAmountX = 1f;
    [SerializeField] private float _snappiness = 10f;
    [SerializeField] private float _recoilRecoverySpeed = 2f;

    private bool _isReloading;

    private int _currentAmmo;
    public int CurrentAmmo => _currentAmmo;
    public int TotalAmmo => _totalAmmo;
    private float _fireCooldownTimer = 0f;
    private float _reloadCooldownTimer = 0f;

    private Coroutine _shootingCoroutine;

    private void Awake()
    {
        _mouseLook = MouseLook.Instance;

        _currentAmmo = _totalAmmo;
        OnAmmoChangedAction?.Invoke(_currentAmmo, _totalAmmo);
    }


    protected virtual void Start()
    {
        
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

        _mouseLook.ApplyRotationModifiers(_snappiness, _recoilRecoverySpeed);
    }

    private void CalculateRecoil()
    {
        float randomXRecoil = Random.Range(-_recoilAmountX, _recoilAmountX);

        _mouseLook.ChangeLookTarget(randomXRecoil, _recoilAmountY);
    }

    public virtual bool Shoot()
    {
        if (CanFire())
        {
            CalculateRecoil();
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
        if (_fireCooldownTimer <= 0f && _currentAmmo > 0 && !_isReloading && _reloadCooldownTimer <= 0f && IsEquipped())
        {
            return true;
        }

        return false;
    }

    public override void UseStart()
    {
        if (WeaponFireMode == FireMode.Semi_Automatic)
        {
            Shoot();
        }
        else if (WeaponFireMode == FireMode.Automatic)
        {
            _shootingCoroutine = StartCoroutine(AutoShootRoutine());
        }
    }

    public override void UseStop()
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
            if (!IsEquipped())
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
            }
            Shoot();

            yield return null;
        }
    }

    private void OnDisable()
    {
        _isReloading = false;
        _reloadCooldownTimer = 0f;

        UseStop();
    }

}

public enum FireMode
{
    Automatic,
    Semi_Automatic
}

public enum BulletType
{
    Single,
    Scatter
}