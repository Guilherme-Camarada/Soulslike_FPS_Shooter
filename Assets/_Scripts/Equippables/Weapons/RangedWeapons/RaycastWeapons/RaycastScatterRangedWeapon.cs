using UnityEngine;

public class RaycastScatterRangedWeapon : RaycastRangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] protected float _range = 50f;
    [SerializeField] protected float _damagePerBullet = 10f;
    [SerializeField] private float _scatterSpreadAngle = 15f;
    [SerializeField] private int _scatterPelletCount = 10;

    [Header("Hit Effects")]
    [SerializeField] protected ParticleSystem _hitParticleSystem;

    public override bool TryShoot()
    {
        if (base.TryShoot())
        {
            MultipleRaycastShot(_damagePerBullet, _range);
            return true;
        }
        return false;
    }

    private void MultipleRaycastShot(float damage, float range)
    {
        for (int i = 0; i < _scatterPelletCount; i++)
        {
            TrailRenderer trail = Instantiate(_trailRenderer, _muzzlePoint.position, Quaternion.identity);
            Vector3 spreadDirection = Quaternion.Euler(Random.Range(-_scatterSpreadAngle, _scatterSpreadAngle), Random.Range(-_scatterSpreadAngle, _scatterSpreadAngle), 0) * _shootOrigin.forward;

            if (Physics.Raycast(_shootOrigin.position, spreadDirection, out RaycastHit hitInfo, range))
            {
                if (hitInfo.collider.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    damageable.TakeDamage(damage);
                }

                StartCoroutine(SpawnTrail(trail, hitInfo.point, hitInfo.normal, true, _hitParticleSystem));
            } else
            {
                StartCoroutine(SpawnTrail(trail, _shootOrigin.position + spreadDirection * range, Vector3.zero, false, null));
            }
        }
    }
}
