using System;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("References")]
    private RangedWeapon _rangedWeapon;
    private AudioSource _audioSource;

    [Header("VisualFX")]
    [SerializeField] protected ParticleSystem _muzzleFlashEffect;

    [Header("SoundFX")]
    [SerializeField] protected AudioClip _fireSound;
    [SerializeField] protected AudioClip _reloadSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rangedWeapon = GetComponent<RangedWeapon>();
    }

    private void OnEnable()
    {
        _rangedWeapon.OnFireAction += RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction += RangedWeapon_OnReloadAction;
    }

    private void RangedWeapon_OnReloadAction(float cooldownTimer)
    {
        if (_reloadSound  != null) 
        {
            float requiredPitch = _reloadSound.length / cooldownTimer;
            _audioSource.pitch = requiredPitch;

            _audioSource.PlayOneShot(_reloadSound);
        }
    }

    private void RangedWeapon_OnFireAction()
    {
        if (_muzzleFlashEffect != null)
        {
            _muzzleFlashEffect.Play();
        }
        if (_fireSound != null)
        {
            _audioSource.PlayOneShot(_fireSound);
        }
    }

    private void OnDisable()
    {
        _rangedWeapon.OnFireAction -= RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction -= RangedWeapon_OnReloadAction;
    }
}
