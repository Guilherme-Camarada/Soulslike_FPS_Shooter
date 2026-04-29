using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

[RequireComponent(typeof(MeleeWeapon))]
public class MeleeWeaponFX : MonoBehaviour
{
    private MeleeWeapon _meleeWeapon;

    [Header("VisualFX")]
    [SerializeField] protected ParticleSystem _strikeEffect;
    [SerializeField] protected ParticleSystem _hitEffect;

    [Header("SoundFX")]
    [SerializeField] protected SoundData _strikeSoundData;
    [SerializeField] protected SoundData _hitSoundData;

    private void Awake()
    {
        _meleeWeapon = GetComponent<MeleeWeapon>();
    }

    private void OnEnable()
    {
        _meleeWeapon.OnWeaponAttack += MeleeWeapon_OnWeaponAttack;
        _meleeWeapon.OnWeaponCollision += MeleeWeapon_OnWeaponCollision;
    }

    private void MeleeWeapon_OnWeaponCollision(ContactPoint point)
    {
        if (_hitEffect != null)
        {
            Instantiate(_hitEffect, point.point, Quaternion.LookRotation(point.normal));
        }

        SoundManager.Instance.CreateSound()
            .WithSoundData(_hitSoundData)
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play();
    }

    private void MeleeWeapon_OnWeaponAttack()
    {
        if (_strikeEffect != null)
        {
            _strikeEffect.Play();
        }
        
        SoundManager.Instance.CreateSound()
            .WithSoundData(_strikeSoundData)
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play();
    }

    private void OnDisable()
    {
        _meleeWeapon.OnWeaponAttack -= MeleeWeapon_OnWeaponAttack;
        _meleeWeapon.OnWeaponCollision -= MeleeWeapon_OnWeaponCollision;
    }
}
