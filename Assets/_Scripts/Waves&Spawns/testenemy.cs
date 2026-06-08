using UnityEngine;
using UnityEngine.AI;

public class testenemy : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform _chaseTarget;
    [SerializeField] private LayerMask _playerLayer;

    private NavMeshAgent _agent;

    [SerializeField] private float _chaseSpeed = 4f;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            _chaseTarget = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the Player tag is set.");
        }
    }

    void Update()
    {
        Chase();
    }

    void Chase()
    {
        if (_chaseTarget == null) return;

        _agent.enabled = true;
        _agent.speed = _chaseSpeed;
        _agent.SetDestination(_chaseTarget.position);
    }
}