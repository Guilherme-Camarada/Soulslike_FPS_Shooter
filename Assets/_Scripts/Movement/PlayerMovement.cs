using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> OnDashAction;
    public event Action OnJumpAction;
    public event Action<float> OnGroundedAction;
    public event Action OnStaminaChangedAction;
    public event Action<bool> OnSprintAction;
    public event Action<bool> OnWalkAction;

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
    public bool IsJumping => _isJumping;
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
            OnSprintAction?.Invoke(false);
        }
    }

    private void GameInput_OnSprintStartAction()
    {
        if (!_isSprinting && _dashCoroutine == null && _inputVector.magnitude > 0)
        {
            _characterSpeed *= _sprintMultiplier;
            _isSprinting = true;
            OnSprintAction?.Invoke(true);
        }
    }

    private void GameInput_OnJumpAction()
    {
        HandlePlayerJump();
    }

    private void OnDisable()
    {
        _gameInput.OnJumpAction -= GameInput_OnJumpAction;
        _gameInput.OnSprintStartAction -= GameInput_OnSprintStartAction;
        _gameInput.OnSprintCancelAction -= GameInput_OnSprintCancelAction;
        _gameInput.OnDashAction -= GameInput_OnDashAction;
    }

    
    private void Start()
    {
        _currentStamina = _maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        _inputVector = GameInput.Instance.GetMovementVectorNormalized();

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

    #region Getters and Setters

    public float GetCharacterSpeed()
    {
        return _characterSpeed;
    }

    public void SetCharacterSpeed(float speed)
    {
        _characterSpeed = speed;
    }

    public float GetSprintMultiplier()
    {
        return _sprintMultiplier;
    }

    public void SetSprintMultiplier(float multiplier)
    {
        _sprintMultiplier = multiplier;
    }

    public float GetCurrentStamina()
    {
        return _currentStamina;
    }

    public float GetMaxStamina()
    {
        return _maxStamina;
    }

    public void SetMaxStamina(float stamina)
    {
        _maxStamina = stamina;
    }

    public float GetStaminaRegenRate()
    {
        return _staminaRegenRate;
    }

    public void SetStaminaRegenRate(float regenRate)
    {
        _staminaRegenRate = regenRate;
    }

    public float GetStaminaDashCost()
    {
        return _staminaDashCost;
    }

    public void SetStaminaDashCost(float dashCost)
    {
        _staminaDashCost = dashCost;
    }

    public float GetDashSpeed()
    {
        return _dashSpeed;
    }

    public void SetDashSpeed(float speed)
    {
        _dashSpeed = speed;
    }

    public float GetDashDistance()
    {
        return _dashDistance;
    }

    public void SetDashDistance(float distance)
    {
        _dashDistance = distance;
    }

    public float GetDashCooldown()
    {
        return _dashCooldown;
    }

    public void SetDashCooldown(float cooldown)
    {
        _dashCooldown = cooldown;
    }
    #endregion

}
