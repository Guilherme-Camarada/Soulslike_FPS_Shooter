using DG.Tweening;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action OnDamageTakenAction;
    public event Action<Damageable> OnDeathAction;

    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        OnDamageTakenAction?.Invoke();
    }

    private void Die() 
    {
        OnDeathAction?.Invoke(this);

        transform.DOKill();

        transform.DOScale(Vector3.zero, 0.5f)
             .SetEase(Ease.InBounce)
             .SetLink(gameObject)
             .OnComplete(() =>
             {
                 if (this != null && gameObject != null)
                 {
                     Destroy(gameObject);
                 }
             });
    }
}
