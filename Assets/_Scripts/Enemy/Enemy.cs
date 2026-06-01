//using System.Collections.Generic;
//using System.Security.Authentication.ExtendedProtection;
//using UnityEngine;
//using UnityEngine.AI;

//[RequireComponent(typeof(Damageable))]
//[RequireComponent(typeof(NavMeshAgent))]
//public class Enemy : MonoBehaviour
//{
//    private Damageable _damageable;
//    private NavMeshAgent _agent;
//    private List<EquipInteractable> _weapon;
//    private EnemyProceduralAnimation _enemyProceduralAnimation;

//    [Header("Target Settings")]
//    [SerializeField] private Transform _chaseTarget;
//    [SerializeField] private LayerMask _playerLayer;

//    [Header("Enemy Settings")]
//    [SerializeField] private float _detectionRadius = 5f;
//    [SerializeField] private float _attackRange = 2.5f;

//    [Header("Patrol Settings")]
//    [SerializeField] private Transform[] _patrolPoints;
//    [SerializeField] private float _patrolSpeed = 2f;

//    [Header("Chase Settings")]
//    [SerializeField] private float _chaseSpeed = 4f;
//    [SerializeField] private float _interestDistance = 10f;

//    private bool _targetInSightRange;
//    private bool _targetInAttackRange;

//    protected virtual void Awake()
//    {
//        _damageable = GetComponent<Damageable>();
//        _agent = GetComponent<NavMeshAgent>();
//        _weapon = new List<EquipInteractable>(GetComponents<EquipInteractable>());
//        _enemyProceduralAnimation = GetComponent<EnemyProceduralAnimation>();
//    }

//    protected virtual void Update()
//    {
//        _targetInSightRange = Physics.CheckSphere(transform.position, _detectionRadius, _playerLayer);
//        _targetInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

//        if (!_targetInSightRange && !_targetInAttackRange)
//        {
//            if (_weapon.Count > 0)
//            {
//                StopAttack();
//            }

//            Patrol();
//        }
//        if (_targetInSightRange && !_targetInAttackRange)
//        {
//            if (_weapon.Count > 0)
//            {
//                StopAttack();
//            }

//            Chase();
//        }
//        if (_targetInSightRange && _targetInAttackRange && _weapon.Count > 0) Attack();

//    }

//    protected virtual void Patrol()
//    {
//        _agent.enabled = true;

//        _agent.stoppingDistance = 0f;

//        if (_patrolPoints.Length > 0)
//        {
//            if (!_agent.hasPath)
//            {
//                Transform nextPatrolPoint = _patrolPoints[Random.Range(0, _patrolPoints.Length)];
//                _agent.SetDestination(nextPatrolPoint.position);
//                _agent.speed = _patrolSpeed;
//            }
//        }
//    }

//    protected virtual void Chase()
//    {
//        _agent.enabled = true;


//        _agent.stoppingDistance = _attackRange;
//        _agent.SetDestination(_chaseTarget.position);
//        _agent.speed = _chaseSpeed;
//    }

//    protected virtual void Attack()
//    {
//        if (_weapon.Count == 0) return;
//        _agent.enabled = false;

//        _enemyProceduralAnimation.SetNeutralStance();

//        Collider targetCollider = _chaseTarget.GetComponent<Collider>();
//        Vector3 targetCenter = targetCollider != null ? targetCollider.bounds.center : _chaseTarget.position;

//        Vector3 direction = (targetCenter - transform.position).normalized;

//        if (direction == Vector3.zero)
//        {
//            foreach (var weapon in _weapon)
//            {
//                weapon.UseStart();
//            }
//        }
//        else
//        {
//            Quaternion lookRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
//            foreach (var weapon in _weapon)
//            {
//                weapon.UseStart();
//            }
//        }
//    }

//    protected virtual void StopAttack()
//    {
//        if (_weapon.Count == 0) return;

//        foreach (var weapon in _weapon)
//        {
//            weapon.UseStop();
//        }

//    }

//}
