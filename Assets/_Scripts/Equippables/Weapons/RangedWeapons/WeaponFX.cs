using System;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("References")]
    private RangedWeapon _rangedWeapon;

    [Header("VisualFX")]
    [SerializeField] protected ParticleSystem _muzzleFlashEffect;
    [SerializeField] private Transform _muzzlePosition;

    [Header("SoundFX")]
    [SerializeField] protected SoundData _fireSoundData;
    [SerializeField] protected SoundData _reloadSoundData;

    private void Awake()
    {
        _rangedWeapon = GetComponent<RangedWeapon>();
    }

    private void OnEnable()
    {
        _rangedWeapon.OnFireAction += RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction += RangedWeapon_OnReloadAction;
    }

    private void RangedWeapon_OnReloadAction(float cooldownTimer)
    {
        if (_reloadSoundData  != null) 
        {
            float requiredPitch = _reloadSoundData._audioClip.length / cooldownTimer;

            SoundManager.Instance.CreateSound()
                .WithSoundData(_reloadSoundData)
                .WithSetPitch(requiredPitch)
                .WithPosition(transform.position)
                .Play();
        }
    }

    private void RangedWeapon_OnFireAction()
    {
        if (_muzzleFlashEffect != null)
        {
            ParticleSystem particleSystem = Instantiate(_muzzleFlashEffect, _muzzlePosition.position, _muzzlePosition.rotation);
            particleSystem.transform.SetParent(_muzzlePosition);
            particleSystem.Play();
        }

        SoundManager.Instance.CreateSound()
            .WithSoundData( _fireSoundData)
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play();    
    }

    private void OnDisable()
    {
        _rangedWeapon.OnFireAction -= RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction -= RangedWeapon_OnReloadAction;
    }
}
