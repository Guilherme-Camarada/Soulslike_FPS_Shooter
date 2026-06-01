using UnityEngine;

public class ProjectileScatterShootUsable : ShootUsable
{
    [Header("Projectile Weapon Settings")]
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileForce = 500f;
    [SerializeField] private float _scatterSpreadAngle = 15f;
    [SerializeField] private int _scatterPelletCount = 10;

    public override bool TryShoot()
    {
        if (base.TryShoot())
        {
            MultipleProjectileShot();
            return true;
        }
        return false;
    }

    private void MultipleProjectileShot()
    {
        for (int i = 0; i < _scatterPelletCount; i++)
        {
            Vector2 randomSpread = Random.insideUnitCircle * _scatterSpreadAngle;
            Vector3 spreadDirection = Quaternion.Euler(randomSpread.x, randomSpread.y, 0) * _muzzlePoint.forward;
            GameObject projectile = Instantiate(_projectilePrefab, _muzzlePoint.position, Quaternion.LookRotation(spreadDirection));
            if (projectile.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForce(spreadDirection * _projectileForce, ForceMode.Impulse);
            }
        }
    }
}
