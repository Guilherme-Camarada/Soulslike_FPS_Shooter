using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RaycastSingleRangedWeapon : RaycastRangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] protected float _range = 50f;
    [SerializeField] protected float _damage = 10f;

    [Header("Hit Effects")]
    [SerializeField] protected ParticleSystem _hitParticleSystem;

    public override bool TryShoot()
    {
        if (base.TryShoot())
        {
            SingleRaycastShot(_damage, _range);
            return true;
        }
        return false;
    }

    private void SingleRaycastShot(float damage, float range)
    {
        TrailRenderer trail = Instantiate(_trailRenderer, _muzzlePoint.position, Quaternion.identity);

        if (Physics.Raycast(_shootOrigin.position, _shootOrigin.forward, out RaycastHit hitInfo, range))
        {
            if (hitInfo.collider.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.TakeDamage(damage);
            }
            
            StartCoroutine(SpawnTrail(trail, hitInfo.point, hitInfo.normal, true, _hitParticleSystem));
        } else
        {
            StartCoroutine(SpawnTrail(trail, _shootOrigin.position + _shootOrigin.forward * range, Vector3.zero, false, null));
        }
    }

    

}
