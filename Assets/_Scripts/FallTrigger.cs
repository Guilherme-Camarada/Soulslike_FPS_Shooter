using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private Transform _teleportTransform;
    [SerializeField] private float _damageDealt;


    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Damageable damageable);

        damageable.TakeDamage(_damageDealt);

        other.transform.position = _teleportTransform.position;
    }
}
