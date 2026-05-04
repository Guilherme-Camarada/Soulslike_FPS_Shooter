using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    private Damageable _damageable;
    private NavMeshAgent _agent;

    [Header("Enemy Settings")]
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _attackRange = 2.5f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _interestDistance = 10f;

    private bool _isPatrolling = true;
    private bool _isChasing;

    private Transform _chaseTarget;

    protected virtual void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (_isPatrolling)
        {
            _isChasing = false;

            _agent.stoppingDistance = 0f;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRadius);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out PlayerInteractor player))
                {
                    _isChasing = true;
                    _isPatrolling = false;  
                    _chaseTarget = player.transform;
                    break;
                }
            }

            if (_patrolPoints.Length > 0)
            {
                if (!_agent.hasPath)
                {
                    Transform nextPatrolPoint = _patrolPoints[Random.Range(0, _patrolPoints.Length)];
                    _agent.SetDestination(nextPatrolPoint.position);
                    _agent.speed = _patrolSpeed;
                }
            }
        }
        else if (_isChasing)
        {
            _isPatrolling = false;

            _agent.stoppingDistance = _attackRange;

            if (_chaseTarget == null || Vector3.Distance(transform.position, _chaseTarget.position) > _interestDistance)
            {
                {
                    _isChasing = false;
                    _isPatrolling = true;
                    return;
                }
            } else
            {
                _agent.SetDestination(_chaseTarget.position);
                _agent.speed = _chaseSpeed;
            }

        }

    }
}
