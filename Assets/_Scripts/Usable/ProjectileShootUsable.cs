using UnityEngine;

public class ProjectileShootUsable : ShootUsable
{
    [Header("Projectile Weapon Settings")]
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileForce = 500f;

    [SerializeField] private float _bonusDamage = 0f;

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
        GameObject projectileInstance = Instantiate(_projectilePrefab, _muzzlePoint.position, _muzzlePoint.rotation);

        if (projectileInstance.TryGetComponent(out Projectile projectile))
        {
            float totalDamage = projectile.GetDamage() + _bonusDamage;
            projectile.SetDamage(totalDamage);
        }

        if (projectile.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(_muzzlePoint.forward * _projectileForce, ForceMode.Impulse);
        }
    }

    public void SetBonusDamage(float amount)
    {
        _bonusDamage = amount;
    }

    public float GetBonusDamage()
    {
        return _bonusDamage;
    }

    public GameObject GetProjectilePrefab()
    {
        return _projectilePrefab;
    }

    public void SetProjectilePrefab(GameObject newProjectile)
    {
        _projectilePrefab = newProjectile;
    }
}
