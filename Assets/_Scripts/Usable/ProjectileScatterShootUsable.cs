using UnityEngine;

public class ProjectileScatterShootUsable : ShootUsable
{
    [Header("Projectile Weapon Settings")]
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileForce = 500f;
    [SerializeField] private float _scatterSpreadAngle = 15f;
    [SerializeField] private int _scatterPelletCount = 10;

    private float _bonusDamage = 0f;

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
            GameObject projectileInstance = Instantiate(_projectilePrefab, _muzzlePoint.position, Quaternion.LookRotation(spreadDirection));

            if (projectileInstance.TryGetComponent(out Projectile projectile))
            {
                float totalDamage = projectile.GetDamage() + _bonusDamage;
                projectile.SetDamage(totalDamage);
            }

            if (projectile.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForce(spreadDirection * _projectileForce, ForceMode.Impulse);
            }
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
