using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] private ParticleSystem _explosionParticleSystem;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _damage;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _powerAmount;

    public override void Execute(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        ParticleSystem systemInstance = Instantiate(_explosionParticleSystem);
        systemInstance.transform.position = transform.position;
        systemInstance.Play();

        SoundManager.Instance.CreateSound()
            .WithSoundData(_soundData)
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play();

        _impulseSource.GenerateImpulse(_powerAmount);

        Destroy(gameObject);
    }

    


}
