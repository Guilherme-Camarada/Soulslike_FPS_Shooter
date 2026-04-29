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

            TrailRenderer trail = Instantiate(_trailRenderer, _muzzlePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitInfo, _hitParticleSystem));
        }
    }

    

}
