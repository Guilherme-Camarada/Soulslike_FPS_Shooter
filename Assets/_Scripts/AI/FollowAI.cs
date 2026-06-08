using System;
using UnityEngine;
using UnityEngine.AI;

public class FollowAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    [Header("Target Settings")]
    private Transform _chaseTarget;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Chase Settings")]
    [SerializeField] private float _chaseSpeed = 4f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.enabled = true;

        _navMeshAgent.SetDestination(_chaseTarget.position);
        _navMeshAgent.speed = _chaseSpeed;
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
    }
}
