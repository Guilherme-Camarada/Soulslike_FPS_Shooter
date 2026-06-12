using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

public class JumpToTargetAI : EnemyAI
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;

    public override void DoAttack()
    {
        JumpAttack(_chaseTarget, _jumpHeight, _jumpDuration);
    }

    private void JumpAttack(Transform target, float height, float duration)
    {
        Vector3 landingPosition = target.position;

        transform.DOJump(landingPosition, _jumpHeight, 1, _jumpDuration).SetLink(gameObject).SetEase(Ease.Linear).OnComplete(() =>
        {
            _navMeshAgent.enabled = true;
            _isAttacking = false;
            _isChasing = true;
        });
    }
}
