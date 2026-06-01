using Unity.VisualScripting;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance { get; private set; }

    [Header("References")]
    private GameInput _gameInput;

    [Header("Player Look Settings")]
    [SerializeField] private Transform _cameraLookAt;
    [SerializeField] private float _mouseSensitivity = 0.1f;
    [SerializeField] private float _upDownLookRange = 80f;
    private float _verticalRotation;

    private Vector3 _targetRotation;
    private Vector3 _currentRotation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        _gameInput = GameInput.Instance;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ApplyRotationModifiers(float snappiness, float recoverySpeed)
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, Time.deltaTime * recoverySpeed);

        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, Time.deltaTime * snappiness);
    }

    private void LateUpdate()
    {
        Vector2 cameraInputVector = GameInput.Instance.GetCameraInputVector();
        HandlePlayerLook(cameraInputVector);
    }

    private void HandlePlayerLook(Vector2 mouseInput)
    {
        HandleHorizontalRotation(mouseInput.x * _mouseSensitivity);
        HandleVerticalRotation(mouseInput.y * _mouseSensitivity);
    }


    private void HandleHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void HandleVerticalRotation(float rotationAmount)
    {
        _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -_upDownLookRange, _upDownLookRange);
        _cameraLookAt.localRotation = Quaternion.Euler(_verticalRotation - _currentRotation.y, _currentRotation.x, 0);
    }

    public void ChangeLookTarget(float targetX, float targetY)
    {
        _targetRotation += new Vector3(-targetX, targetY, 0f);
    }

    public Vector3 GetCameraLookDirection()
    {
        return _cameraLookAt.forward;
    }
}
