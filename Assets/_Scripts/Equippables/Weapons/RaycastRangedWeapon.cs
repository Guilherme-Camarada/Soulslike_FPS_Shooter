using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RaycastRangedWeapon : RangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] protected float _range = 50f;
    [SerializeField] protected float _damage = 10f;

    [Header("Hit Effects")]
    [SerializeField] protected ParticleSystem _hitParticleSystem;
    [SerializeField] protected TrailRenderer _trailRenderer;
    [SerializeField] protected Transform _muzzlePoint;

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
            StartCoroutine(SpawnTrail(trail, hitInfo));
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit targetPoint)
    {
        Vector3 startPosition = trail.transform.position;
        float time = 0f;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = targetPoint.point;
        Instantiate(_hitParticleSystem, targetPoint.point, Quaternion.LookRotation(targetPoint.normal));

        Destroy(trail.gameObject, trail.time);
    }

    

}
