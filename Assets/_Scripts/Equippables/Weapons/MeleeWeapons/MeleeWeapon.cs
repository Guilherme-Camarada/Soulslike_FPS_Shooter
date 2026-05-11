using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class MeleeWeapon : Usable
{
    public event Action OnWeaponAttack;
    public event Action<ContactPoint> OnWeaponCollision;

    private Collider _weaponCollider;

    [Header("Stats")]
    [SerializeField] private float _damage;

    [Header("Swing Timings")]
    [SerializeField] private float _attackSpeedMultiplier = 1f;
    [SerializeField] private float _windupTime = 0.15f;
    [SerializeField] private float _strikeTime = 0.20f;
    [SerializeField] private float _recoveryTime = 0.25f;

    public float AttackSpeedMultiplier => _attackSpeedMultiplier;
    public float WindupTime => _windupTime;
    public float RecoveryTime => _recoveryTime;
    public float StrikeTime => _strikeTime;

    public bool IsSwinging { get; set; } = false;

    private void Awake()
    {
        _weaponCollider = GetComponent<Collider>();    
    }

    private void Start()
    {
        _weaponCollider.enabled = false;
    }

    public override void UseStart()
    {
        if (!IsSwinging)
        {
            IsSwinging = true;
            _weaponCollider.enabled = true;
            OnWeaponAttack?.Invoke();
        }
    }

    public override void UseStop()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        _weaponCollider.enabled = false;

        if (collision.collider.TryGetComponent(out Damageable damageable))
        {
            OnWeaponCollision?.Invoke(collision.GetContact(0));
            damageable.TakeDamage(_damage);
        }
    }
}
