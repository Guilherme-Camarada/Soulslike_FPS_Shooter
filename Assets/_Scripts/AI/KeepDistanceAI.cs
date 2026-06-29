using DG.Tweening;
using PixPlays.ElementalVFX;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class KeepDistanceAI : EnemyAI
{
    [SerializeField] private float _runAwayDistance = 5f;
    [SerializeField] private float _fleeSpeed = 6f;
    [SerializeField] private float _escapePushDistance = 10f;

    private bool _isRunningAway;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileTravelDuration;
    [SerializeField] private float _projectileHeight;
    [SerializeField] private float _attackCooldown = 1.5f;

    public override void DoAttack()
    {
        StartCoroutine(DoRangedAttack(_chaseTarget, _projectileTravelDuration, _projectileHeight));
    }


    //protected override void Update()
    //{
    //    if (_chaseTarget == null) return;

    //    if (_isAttacking) return;

    //    float distanceSquared = (_chaseTarget.position - transform.position).sqrMagnitude;
    //    float runAwayRadiusSquared = _runAwayDistance * _runAwayDistance;

    //    if (distanceSquared <= runAwayRadiusSquared)
    //    {
    //        RunAway(_chaseTarget, _escapePushDistance);
    //    }
    //    else
    //    {
    //        base.Update();
    //    }
    //}

    private void RunAway(Transform target, float distance)
    {
        _isChasing = false;

        Vector3 directionAwayFromTarget = (transform.position - target.position).normalized;

        Vector3 targetFleePosition = transform.position + (directionAwayFromTarget * distance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetFleePosition, out hit, 5f, NavMesh.AllAreas))
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(hit.position);
            _navMeshAgent.speed = _fleeSpeed;
        }
    }

    private IEnumerator DoRangedAttack(Transform target, float projectileSpeed, float projectileHeight)
    {
        Debug.Log("Doing Ranged Attack");
        GameObject instantiatedProjectile = Instantiate(_projectilePrefab);
        instantiatedProjectile.transform.position = transform.position + Vector3.up * 1f;
        instantiatedProjectile.transform.rotation = transform.rotation;

        instantiatedProjectile.transform.DOJump(target.position, _projectileHeight, 1, _projectileTravelDuration)
            .SetEase(Ease.Linear)
            .SetLink(instantiatedProjectile)
            .OnComplete(() =>
            {
                _navMeshAgent.enabled = true;
                _isAttacking = false;
                _isChasing = true;
                Destroy(instantiatedProjectile, 0.15f);
            });

        yield return new WaitForSeconds(_attackCooldown);

        _navMeshAgent.enabled = true;
        _isAttacking = false;
        _isChasing = true;
    }
}
