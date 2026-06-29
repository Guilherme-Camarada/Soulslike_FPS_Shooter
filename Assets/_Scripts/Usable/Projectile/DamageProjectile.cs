using UnityEngine;

public class DamageProjectile : Projectile
{
    public override void Execute(Collider other)
    {
        if ((_damageableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.transform.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        base.Execute(other);
    }
}
