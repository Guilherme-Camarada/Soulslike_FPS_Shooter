using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraFOVChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private CinemachineCamera _cinemachineWeaponCamera;

    [Header("FOV Sprint Settings")]
    [SerializeField] private float _defaultFOV = 60f;
    [SerializeField] private float _sprintFOV = 70f;
    [SerializeField] private float _sprintFOVChangeDuration = 0.5f;

    [Header("FOV Dash Settings")]
    [SerializeField] private float _dashFOV = 80f;
    [SerializeField] private float _dashPunchDuration = 0.1f;
    [SerializeField] private float _dashRecoveryDuration = 0.4f;

    private Tween _currentFOVTween;


    private void OnEnable()
    {
        _playerMovement.OnDashAction += PlayerMovement_OnDashAction;
        _playerMovement.OnSprintAction += PlayerMovement_OnSprintAction;
        _playerMovement.OnWalkAction += PlayerMovement_OnWalkAction;
    }

    private void PlayerMovement_OnWalkAction(bool isWalking)
    {
        if (!isWalking)
        {
            if (_cinemachineCamera.Lens.FieldOfView < _dashFOV - 1f)
            {
                Debug.Log("Not walking, applying default FOV");
                ApplyFOVChange(_defaultFOV, _sprintFOVChangeDuration, Ease.InOutSine);
            }
        }
    }

    private void PlayerMovement_OnSprintAction(bool isSprinting)
    {
        if (!isSprinting && _cinemachineCamera.Lens.FieldOfView < _dashFOV - 1f)
        {
            Debug.Log("Not sprinting, applying default FOV");
            ApplyFOVChange(_defaultFOV, _sprintFOVChangeDuration, Ease.InOutSine);
        }
        else if (isSprinting && _cinemachineCamera.Lens.FieldOfView < _dashFOV - 1f)
        {
            Debug.Log("Sprinting, applying sprint FOV");
            ApplyFOVChange(_sprintFOV, _sprintFOVChangeDuration, Ease.InOutSine);
        }
    }

    private void PlayerMovement_OnDashAction(bool isDashing)
    {
        if (isDashing)
        {
            Debug.Log("Dashing, applying dash FOV");
            ApplyFOVChange(_dashFOV, _dashPunchDuration, Ease.InOutSine);
        } else
        {
            Debug.Log("Finished dashing, applying recovery FOV");
            float targetFOV = _playerMovement.IsSprinting() ? _sprintFOV : _defaultFOV;
            ApplyFOVChange(targetFOV, _dashRecoveryDuration, Ease.InOutSine);
        }
    }

    private void OnDisable()
    {
        _playerMovement.OnDashAction -= PlayerMovement_OnDashAction;
        _playerMovement.OnSprintAction -= PlayerMovement_OnSprintAction;
        _playerMovement.OnWalkAction -= PlayerMovement_OnWalkAction;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineCamera.Lens.FieldOfView = _defaultFOV;
        _cinemachineWeaponCamera.Lens.FieldOfView = _defaultFOV;
    }

    private void Update()
    {
        
    }


    private void ApplyFOVChange(float value, float duration, Ease easeType)
    {
        _currentFOVTween?.Kill();

        _currentFOVTween = DOTween.To(
            () => _cinemachineCamera.Lens.FieldOfView, 
            x => 
            {
                _cinemachineCamera.Lens.FieldOfView = x;
                _cinemachineWeaponCamera.Lens.FieldOfView = x;
            }, value, duration)
            .SetEase(easeType);
    }

}
