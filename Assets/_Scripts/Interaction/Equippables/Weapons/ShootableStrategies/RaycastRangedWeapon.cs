using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RaycastRangedWeapon : RangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] private float _impactForce = 50f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool Shoot(float damage, float range)
    {
        if (WeaponBulletType == BulletType.Scatter)
        {
            if (base.Shoot(damage, range))
            {
                MultipleRaycastShot(damage, range, 10, 15f);
                return true;
            }
            return false;
        } else if (WeaponBulletType == BulletType.Single)
        {
            if (base.Shoot(damage, range))
            {
                SingleRaycastShot(damage, range);
                return true;
            }
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

            if (hitInfo.collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddForce(-hitInfo.normal * _impactForce, ForceMode.Impulse);
            }
        }
    }

    private void MultipleRaycastShot(float damage, float range, float pelletCount, float spreadAngle)
    {
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 spreadDirection = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0) * Camera.main.transform.forward;
            if (Physics.Raycast(Camera.main.transform.position, spreadDirection, out RaycastHit hitInfo, range))
            {
                if (hitInfo.collider.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
                if (hitInfo.collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    rigidbody.AddForce(-hitInfo.normal * _impactForce, ForceMode.Impulse);
                }
            }
        }
    }

}
