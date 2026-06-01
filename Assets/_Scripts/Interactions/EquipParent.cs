using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipParent : MonoBehaviour
{
    public event Action<EquipInteractable, EquipInteractable> OnEquipInteractableChangedAction;

    private EquipInteractable _equipInteractable;
    [SerializeField] private Transform _equipTargetTransform;
    public Transform EquipTargetTransform => _equipTargetTransform;


    private void OnEnable()
    {
        GameInput.Instance.OnDropAction += GameInput_OnDropAction;
        GameInput.Instance.OnUseStartAction += GameInput_OnUseStartAction;
        GameInput.Instance.OnUseCancelAction += GameInput_OnUseCancelAction;
        GameInput.Instance.OnReloadAction += GameInput_OnReloadAction;
    }

    private void GameInput_OnReloadAction()
    {
        if (_equipInteractable != null)
        {
            if (_equipInteractable.TryGetComponent(out ShootUsable shootUsable))
            {
                shootUsable.Reload();
            }
        }
    }

    private void GameInput_OnUseStartAction()
    {
        if (_equipInteractable != null)
        {
            if (_equipInteractable.TryGetComponent(out Usable usable))
            {
                usable.UseStart();
            }
        }
    }

    private void GameInput_OnUseCancelAction()
    {
        if (_equipInteractable != null)
        {
            if (_equipInteractable.TryGetComponent(out Usable usable))
            {
                usable.UseStop();
            }
        }
    }

    private void GameInput_OnDropAction()
    {
        if (_equipInteractable == null) return;

        _equipInteractable.SetEquipParent(null);

        MouseLook mouseLook = MouseLook.Instance;

        _equipInteractable.TryGetComponent(out Rigidbody rigidbody);

        rigidbody.isKinematic = false;

        Vector3 dropDirection = mouseLook.GetCameraLookDirection();

        float throwHeight = 0.35f;

        dropDirection += Vector3.up * throwHeight;

        float throwForce = 35f;
        float throwTorque = 1f;

        rigidbody.AddForce(dropDirection.normalized * throwForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-5f, 5f), Random.Range(-1f, 1f)).normalized * throwTorque;

        rigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        OnEquipInteractableChangedAction?.Invoke(_equipInteractable, null);

        _equipInteractable = null;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnDropAction -= GameInput_OnDropAction;
        GameInput.Instance.OnUseStartAction -= GameInput_OnUseStartAction;
        GameInput.Instance.OnUseCancelAction -= GameInput_OnUseCancelAction;
    }

    public void SetEquipInteractable(EquipInteractable equipInteractable)
    {
        OnEquipInteractableChangedAction?.Invoke(_equipInteractable, equipInteractable);
        _equipInteractable = equipInteractable;  
    }

    public bool HasEquipInteractable()
    {
        return _equipInteractable != null;
    }
}
