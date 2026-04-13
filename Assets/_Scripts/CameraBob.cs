using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Camera Bob Settings")]
    [SerializeField] private float _frequency = 12f;
    [SerializeField] private float _amplitudeX = 0.015f;
    [SerializeField] private float _amplitudeY = 0.015f;
    [SerializeField] private float _bobSmoothness = 10f;
    [SerializeField] private float _pullBackSmoothness = 5f;
    [SerializeField] private float _sprintBobMultiplier = 1.5f;

    [Header("Hands Bob")]
    [SerializeField] private Transform _handsContainer;
    [SerializeField] private float _handsMultiplier = -0.5f;

    private Vector3 _visionStartPosition;
    private Vector3 _handsStartPosition;
    private float _bobTimer;

    void Start()
    {
        _visionStartPosition = transform.localPosition;

        if (_handsContainer != null)
        {
            _handsStartPosition = _handsContainer.localPosition;
        }
    }

    void Update()
    {
        if (_playerMovement.IsSprinting() && _playerMovement.IsGrounded())
        {
            ApplyBob(_sprintBobMultiplier);
        }
        else if(_playerMovement.IsWalking() && _playerMovement.IsGrounded())
        {
            ApplyBob();
        } 
        else
        {
            StopBob();
        }
    }

    private void ApplyBob(float multiplier = 1f)
    {
        _bobTimer += Time.deltaTime * _frequency * multiplier;

        float xOffset = Mathf.Cos(_bobTimer / 2f) * _amplitudeX * multiplier;
        float yOffset = Mathf.Sin(_bobTimer) * _amplitudeY * multiplier;

        Vector3 visionTarget = _visionStartPosition + new Vector3(xOffset, yOffset, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, visionTarget, _bobSmoothness * Time.deltaTime);

        if (_handsContainer != null)
        {
            Vector3 handsTarget = _handsStartPosition + new Vector3(xOffset, yOffset, 0f) * _handsMultiplier;
            _handsContainer.localPosition = Vector3.Lerp(_handsContainer.localPosition, handsTarget, _bobSmoothness * Time.deltaTime);
        }
    }

    private void StopBob()
    {
        if (transform.localPosition != _visionStartPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _visionStartPosition, _pullBackSmoothness * Time.deltaTime);
        }

        if (_handsContainer != null && _handsContainer.localPosition != _handsStartPosition)
        {
            _handsContainer.localPosition = Vector3.Lerp(_handsContainer.localPosition, _handsStartPosition, _pullBackSmoothness * Time.deltaTime);
        }

        if (Vector3.Distance(transform.localPosition, _visionStartPosition) < 0.001f)
        {
            _bobTimer = 0f;
        }
    }
}