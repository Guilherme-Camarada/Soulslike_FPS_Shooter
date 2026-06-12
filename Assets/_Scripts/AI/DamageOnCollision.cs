using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _damageableLayer;
    [SerializeField] private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        if ((_damageableLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.gameObject.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
    }
}
