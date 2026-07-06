using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event Action OnJumpAction;

    public event Action OnInteractAction;

    public event Action OnUseStartAction;
    public event Action OnUseCancelAction;

    public event Action OnLookAction;

    public event Action OnReloadAction;    

    public event Action OnDropAction;

    public event Action OnSprintStartAction;
    public event Action OnSprintCancelAction;

    public event Action OnDashAction;

    public event Action OnPauseAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Jump.performed += ctx => OnJumpAction?.Invoke();

        _playerInputActions.Player.Interact.performed += ctx => OnInteractAction?.Invoke();

        _playerInputActions.Player.Use.started += ctx => OnUseStartAction?.Invoke();
        _playerInputActions.Player.Use.canceled += ctx => OnUseCancelAction?.Invoke();

        _playerInputActions.Player.WeaponLook.performed += ctx => OnLookAction?.Invoke();

        _playerInputActions.Player.Reload.performed += ctx => OnReloadAction?.Invoke();

        _playerInputActions.Player.Drop.performed += ctx => OnDropAction?.Invoke();

        _playerInputActions.Player.Sprint.started += ctx => OnSprintStartAction?.Invoke();
        _playerInputActions.Player.Sprint.canceled += ctx => OnSprintCancelAction?.Invoke();

        _playerInputActions.Player.Dash.performed += ctx => OnDashAction?.Invoke();

        _playerInputActions.Player.Pause.performed += ctx => OnPauseAction?.Invoke();
    }

    private void OnEnable()
    {
        if (_playerInputActions != null)
        {
            _playerInputActions.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (_playerInputActions != null)
        {
            _playerInputActions.Player.Disable();
        }
    }

    public Vector2 GetMouseScrollInputVector()
    {
        Vector2 mouseScrollInputVector = _playerInputActions.Player.MouseScroll.ReadValue<Vector2>();

        return mouseScrollInputVector;
    }

    public Vector2 GetCameraInputVector()
    {
        Vector2 cameraInputVector = _playerInputActions.Player.MouseLook.ReadValue<Vector2>();

        return cameraInputVector;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetMouseMovement()
    {
        Vector2 mouseMovement = _playerInputActions.Player.MouseLook.ReadValue<Vector2>();

        return mouseMovement;
    }
}
