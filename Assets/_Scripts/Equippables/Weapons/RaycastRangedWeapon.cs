using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RaycastRangedWeapon : RangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] protected float _range = 50f;
    [SerializeField] protected float _damage = 10f;

    public override bool Shoot()
    {
        if (base.Shoot())
        {
            SingleRaycastShot(_damage, _range);
            return true;
        }
        return false;
    }

    private void SingleRaycastShot(float damage, float range)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, range))
        {
            if (hitInfo.collider.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    

}
