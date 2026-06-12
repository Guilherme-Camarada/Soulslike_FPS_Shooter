using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent _navMeshAgent;
    private Damageable _damageable;

    [Header("Target Settings")]
    protected Transform _chaseTarget;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Chase Settings")]
    [SerializeField] private float _chaseSpeed = 4f;

    [Header("Attack Settings")]
    [SerializeField] private float _attackRadius;

    protected bool _isAttacking;
    protected bool _isChasing = true;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnDeathAction += Damageable_OnDeathAction;
    }

    private void Damageable_OnDeathAction(Damageable obj)
    {
        _navMeshAgent.enabled = false;
    }

    private void OnDisable()
    {
        _damageable.OnDeathAction -= Damageable_OnDeathAction;
    }

    private void Start()
    {
        _navMeshAgent.stoppingDistance = _attackRadius;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_navMeshAgent == null || _chaseTarget == null) return;

        if (_isAttacking) return;

        float distanceSquared = (_chaseTarget.position - transform.position).sqrMagnitude;
        float attackRadiusSquared = _attackRadius * _attackRadius;

        bool isPlayerInAttackRange = distanceSquared <= attackRadiusSquared;

        if (isPlayerInAttackRange)
        {
            _isAttacking = true;
        } else
        {
            _isChasing = true;
        }

        if (_isAttacking) Attack();
        else if (_isChasing) Chase(_chaseTarget);
        
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
    }

    public abstract void DoAttack();

    private void Chase(Transform target)
    {
        _isAttacking = false;

        _navMeshAgent.enabled = true;

        _navMeshAgent.SetDestination(target.position);
        _navMeshAgent.speed = _chaseSpeed;
    }

    private void Attack()
    {
        _isChasing = false;

        _navMeshAgent.enabled = false;

        DoAttack();
    }
}
