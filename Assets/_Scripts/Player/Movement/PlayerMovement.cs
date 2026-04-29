using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> OnDashAction;
    public event Action OnJumpAction;
    public event Action<float> OnGroundedAction;
    public event Action OnStaminaChangedAction;

    [Header("References")]
    private GameInput _gameInput;
    private CharacterController _characterController;

    [Header("Player Movement Settings")]
    [SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private float _sprintMultiplier = 1.5f;

    [Header("Player Stamina Settings")]
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaRegenRate = 10f;
    [SerializeField] private float _staminaDashCost = 30f;

    public float CurrentStamina => _currentStamina;
    public float MaxStamina => _maxStamina;

    private float _currentStamina;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _wasGrounded;
    private Vector2 _inputVector;
    private bool _isSprinting;
    private bool _isJumping;
    private float _timeInAir;

    [Header("Player Gravity Settings")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravityValue = -9.81f;

    [Header("Player Dash Settings")]
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDistance = 2f;
    [SerializeField] private float _dashCooldown = 0.25f;
    private Coroutine _dashCoroutine;



    private void Awake()
    {
        _gameInput = GameInput.Instance;
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _gameInput.OnJumpAction += GameInput_OnJumpAction;
        _gameInput.OnSprintStartAction += GameInput_OnSprintStartAction;
        _gameInput.OnSprintCancelAction += GameInput_OnSprintCancelAction;
        _gameInput.OnDashAction += GameInput_OnDashAction;
    }

    private void GameInput_OnDashAction()
    {
        if (_dashCoroutine == null && _inputVector.magnitude > 0 && _currentStamina >= _staminaDashCost)
        {
            _dashCoroutine = StartCoroutine(DashCoroutine(_inputVector));
            OnDashAction?.Invoke(true);
        }
    }

    private void GameInput_OnSprintCancelAction()
    {
        if (_isSprinting)
        {
            _characterSpeed /= _sprintMultiplier;
            _isSprinting = false;
        }
    }

    private void GameInput_OnSprintStartAction()
    {
        if (!_isSprinting && _dashCoroutine == null && _inputVector.magnitude > 0)
        {
            _characterSpeed *= _sprintMultiplier;
            _isSprinting = true;
        }
    }

    private void OnDisable()
    {
        _gameInput.OnJumpAction -= GameInput_OnJumpAction;
    }

    private void GameInput_OnJumpAction()
    {
        HandlePlayerJump();
    }

    private void Start()
    {
        _currentStamina = _maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        _inputVector = _gameInput.GetMovementVectorNormalized();

        _wasGrounded = _isGrounded;
        _isGrounded = _characterController.isGrounded;

        if (!_isGrounded)
        {
            _timeInAir += Time.deltaTime;
        }

        if (!_wasGrounded && _isGrounded)
        {
            OnGroundedAction?.Invoke(_timeInAir);
            _timeInAir = 0f;
        }

        HandlePlayerMovement(_inputVector);

        if (_isJumping && _isGrounded)
        {
            _isJumping = false;
            OnJumpAction?.Invoke();
        }

        if (!IsWalking())
        {
            GameInput_OnSprintCancelAction();
        }

        if (_dashCoroutine == null)
        {
            _currentStamina = Mathf.Clamp(_currentStamina + _staminaRegenRate * Time.deltaTime, 0f, _maxStamina);
            OnStaminaChangedAction?.Invoke();
        }
    }

   
    private void HandlePlayerJump()
    {
        if (_isGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            _isJumping = true;
            OnJumpAction?.Invoke();
        }
    }

    private void HandlePlayerMovement(Vector2 inputVector)
    {
        //Grounded Movement
        Quaternion flatRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        Vector3 moveDirection = flatRotation * new Vector3(inputVector.x, 0f, inputVector.y);
        _characterController.Move(_characterSpeed * Time.deltaTime * moveDirection);

        //Jump and Gravity
        _playerVelocity.y += _gravityValue * Time.deltaTime;

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private IEnumerator DashCoroutine(Vector2 inputDirection)
    {
        float distanceTraveled = 0f;
        while (distanceTraveled < _dashDistance)
        {
            Quaternion flatRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            Vector3 dashDirection = flatRotation * new Vector3(inputDirection.x, 0f, inputDirection.y);
            _characterController.Move(_dashSpeed * Time.deltaTime * dashDirection);

            distanceTraveled += _dashSpeed * Time.deltaTime;
            yield return null;
        }

        _currentStamina -= _staminaDashCost;
        OnStaminaChangedAction?.Invoke();
        OnDashAction?.Invoke(false);
        yield return new WaitForSeconds(_dashCooldown);
        _dashCoroutine = null;
    }

    public bool IsWalking()
    {
        return _inputVector.magnitude > 0;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public bool IsSprinting()
    {
        return _isSprinting;
    }   

    public bool IsDashing()
    {
        return _dashCoroutine != null;
    }
}
