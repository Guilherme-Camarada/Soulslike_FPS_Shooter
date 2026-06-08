using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyProceduralAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> _legsIKTargets;
    [SerializeField] private List<Transform> _legsWalkTargets;

    [SerializeField] private float _stepDistance = 1f;
    [SerializeField] private float _stepHeight = 0.5f;
    [SerializeField] private float _stepDuration = 0.5f;

    private List<Vector3> _legsPlantedPositions;

    private bool[] _isStepping;

    private void Start()
    {
        _legsPlantedPositions = new List<Vector3>();
        _isStepping = new bool[_legsIKTargets.Count];

        foreach (Transform target in _legsIKTargets)
        {
            _legsPlantedPositions.Add(target.position);
        }

    }

    private void Update()
    {
        for (int i = 0; i < _legsIKTargets.Count; i++)
        {
            if (_isStepping[i]) continue;

            _legsIKTargets[i].position = _legsPlantedPositions[i];

            Vector3 rayOrigin = _legsWalkTargets[i].position + (Vector3.up * 2f);
            Ray ray = new Ray(rayOrigin, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                if (Vector3.Distance(_legsPlantedPositions[i], hit.point) > _stepDistance)
                {
                    TakeStep(i, hit.point);
                }
            }
        }
    }

    private void TakeStep(int index, Vector3 targetPosition)
    {
        _isStepping[index] = true;

        _legsPlantedPositions[index] = targetPosition;

        _legsIKTargets[index].DOJump(targetPosition, _stepHeight, 1, _stepDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _isStepping[index] = false;
            });
    }

    public void SetNeutralStance()
    {
        for (int i = 0; i < _legsIKTargets.Count; i++)
        {
            if (_isStepping[i]) continue;

            Vector3 rayOrigin = _legsWalkTargets[i].position + (Vector3.up * 2f);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f))
            {
                if (Vector3.Distance(_legsPlantedPositions[i], hit.point) > 0.3f)
                {
                    TakeStep(i, hit.point);
                }
            }
        }
    }
}

