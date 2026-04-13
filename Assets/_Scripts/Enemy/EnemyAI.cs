using UnityEditor.XR;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private Damageable _damageable;

    private Animator _animator;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionRange = 10f;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Attack Settings")]
    [SerializeField] private float _attackDamage = 10f;

    private bool _hasDestination = false;

    private Damageable _currentTargetDamageable;

    private void OnEnable()
    {
        _damageable.OnDeathAction += Damageable_OnDeath;
    }

    private void OnDisable()
    {
        _damageable.OnDeathAction -= Damageable_OnDeath;
    }

    private void Damageable_OnDeath()
    {
        _navMeshAgent.enabled = false;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
    }

    public void OnInit(PlayerInteractor playerInteractor)
    {
        _target = playerInteractor.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasDestination)
        {
            _navMeshAgent.SetDestination(_target.position);
            _hasDestination = true;
            _animator.SetBool("isWalking", true);
        }

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hitInfo, _detectionRange))
        {
            if (hitInfo.collider.TryGetComponent(out Damageable damageable))
            {
                _currentTargetDamageable = damageable;
                _navMeshAgent.isStopped = true;
                _animator.SetBool("isAttacking", true);
                _animator.SetBool("isWalking", false);
            } else
            {
                _currentTargetDamageable = null;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.position);

                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    FaceTarget();
                }

                _animator.SetBool("isAttacking", false);
                _animator.SetBool("isWalking", true);
            }
        } else
        {
            _currentTargetDamageable = null;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_target.position);

            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                FaceTarget();
            }

            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isWalking", true);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;

        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
    }

    public void Attack()
    {
        _currentTargetDamageable.TakeDamage(_attackDamage);
    }

    private void OnAnimatorMove()
    {
        if (_animator != null)
        {
            if (_animator.GetBool("isWalking"))
            {
                _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
    }
}
