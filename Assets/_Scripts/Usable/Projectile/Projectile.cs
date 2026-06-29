using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected LayerMask _damageableLayers;
    [SerializeField] protected SoundData _hitSoundData;
    [SerializeField] protected ParticleSystem _hitParticleSystem;

    [SerializeField] protected float _damage;
    [SerializeField] protected float _pierce;
    private float _currentPiercedTargets;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public virtual void Execute(Collider other)
    {
        if (_hitParticleSystem != null)
        {
            ParticleSystem systemInstance = Instantiate(_hitParticleSystem);
            systemInstance.transform.position = transform.position;
            systemInstance.Play();
        }

        if (_hitSoundData != null)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(_hitSoundData)
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play();
        }

        _currentPiercedTargets++;
        if (_currentPiercedTargets > _pierce)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Execute(other);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public float GetDamage()
    {
        return _damage;
    }
}
