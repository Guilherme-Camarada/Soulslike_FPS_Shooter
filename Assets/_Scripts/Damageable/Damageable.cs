using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
    public event Action OnDamageTakenAction;
    public event Action<Damageable> OnDeathAction;

    [SerializeField] private float _invicibilityDuration = 0.5f;
    private bool _isInvincible;

    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private IEnumerator TriggerInvincibility(float duration)
    {
        _isInvincible = true;

        yield return new WaitForSeconds(duration);

        _isInvincible = false;
    }

    public void TakeDamage(float damageAmount)
    {
        if (_isInvincible) return;

        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(TriggerInvincibility(_invicibilityDuration));

        OnDamageTakenAction?.Invoke();
    }

    private void Die() 
    {
        OnDeathAction?.Invoke(this);

        transform.DOKill();

        transform.DOScale(Vector3.zero, 0.5f)
             .SetEase(Ease.Linear)
             .SetLink(gameObject)
             .OnComplete(() =>
             {
                 if (this != null && gameObject != null)
                 {
                     Destroy(gameObject);
                 }
             });
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float newMax)
    {
        maxHealth = newMax;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void SetCurrentHealth(float newHealth)
    {
        _currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
    }
}
