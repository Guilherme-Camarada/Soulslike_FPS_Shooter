using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _powerAmount;

    public override void Execute(Collider other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        _impulseSource.GenerateImpulse(_powerAmount);

        base.Execute(other);
    }

    


}
