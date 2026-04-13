using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private Damageable _damageable;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    private Rigidbody _rigidbody;

    [SerializeField] private List<Rigidbody> _ragdollRigidbodies;

    private void OnEnable()
    {
        _damageable.OnDeathAction += Damageable_OnDeath;
    }

    private void OnDisable()
    {
        _damageable.OnDeathAction -= Damageable_OnDeath;
    }

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Damageable_OnDeath()
    {
        ActivateRagdoll();
    }

    private void ActivateRagdoll()
    {
        _capsuleCollider.enabled = false;
        _animator.enabled = false;

        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    private void DeactivateRagdoll()
    {
        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        _capsuleCollider.enabled = true;
        _animator.enabled = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeactivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
