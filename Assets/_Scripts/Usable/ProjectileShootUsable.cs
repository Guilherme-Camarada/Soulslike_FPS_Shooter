using UnityEngine;

public class ProjectileShootUsable : ShootUsable
{
    [Header("Projectile Weapon Settings")]
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileForce = 500f;

    public override bool TryShoot()
    {
        if (base.TryShoot())
        {
            SingleProjectileShot();
            return true;
        }
        return false;
    }

    private void SingleProjectileShot()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _muzzlePoint.position, _muzzlePoint.rotation);
        if (projectile.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(_muzzlePoint.forward * _projectileForce, ForceMode.Impulse);
        }
    }
}
