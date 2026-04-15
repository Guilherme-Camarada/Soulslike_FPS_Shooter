using UnityEngine;

public class RaycastScatterRangedWeapon : RangedWeapon
{
    [Header("Raycast Weapon Settings")]
    [SerializeField] protected float _range = 50f;
    [SerializeField] protected float _damagePerBullet = 10f;
    [SerializeField] private float _scatterSpreadAngle = 15f;
    [SerializeField] private int _scatterPelletCount = 10;

    public override bool Shoot()
    {
        if (base.Shoot())
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
            Vector3 spreadDirection = Quaternion.Euler(Random.Range(-_scatterSpreadAngle, _scatterSpreadAngle), Random.Range(-_scatterSpreadAngle, _scatterSpreadAngle), 0) * Camera.main.transform.forward;
            if (Physics.Raycast(Camera.main.transform.position, spreadDirection, out RaycastHit hitInfo, range))
            {
                if (hitInfo.collider.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
