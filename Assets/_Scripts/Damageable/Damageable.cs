using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action OnDamageTakenAction;
    public event Action OnDeathAction;

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
        if (_currentHealth <= 0)
        {
            Die();
            OnDeathAction?.Invoke();    
            return;
        }

        _currentHealth -= damageAmount;
        OnDamageTakenAction?.Invoke();
    }

    private void Die() 
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
