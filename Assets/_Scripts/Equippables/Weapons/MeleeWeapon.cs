using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeWeapon : Equippable
{
    public event Action OnWeaponAttack;
    public event Action<ContactPoint> OnWeaponCollision;

    private Collider _weaponCollider;

    [Header("Stats")]
    [SerializeField] private float _damage;

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
        _weaponCollider.enabled = true;
        OnWeaponAttack?.Invoke();
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
