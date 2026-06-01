using System;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public event Action<Interactable> OnLookAtInteractableChanged;

    [Header("Interaction Settings")]
    [SerializeField] private Transform _cameraLookAt;
    [SerializeField] private float _interactionRange = 2.5f;
    [SerializeField] private LayerMask _interactionLayerMask;

    private Interactable _currentLookAtInteractable;
    public Interactable CurrentLookAtInteractable => _currentLookAtInteractable;

    void Update()
    {
        HandlePlayerInteractions(_interactionRange);
    }

    private void HandlePlayerInteractions(float interactionRange)
    {
        if (Physics.Raycast(_cameraLookAt.position, _cameraLookAt.forward, out RaycastHit hitInfo, interactionRange, _interactionLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out Interactable interactable))
            {
                if (hitInfo.collider.TryGetComponent(out EquipInteractable equipInteractable))
                {
                    if (equipInteractable.IsEquipped) return;
                }

                if (_currentLookAtInteractable == null)
                {
                    _currentLookAtInteractable = interactable;
                    _currentLookAtInteractable.IsSelected = true;
                    OnLookAtInteractableChanged?.Invoke(interactable);
                } else if (_currentLookAtInteractable != null && _currentLookAtInteractable != interactable)
                {
                    _currentLookAtInteractable.IsSelected = false;
                    _currentLookAtInteractable = interactable;
                    _currentLookAtInteractable.IsSelected = true;
                    OnLookAtInteractableChanged?.Invoke(interactable);
                }    
            }
            else
            {
                if (_currentLookAtInteractable != null)
                {
                    _currentLookAtInteractable.IsSelected = false;
                    _currentLookAtInteractable = null;
                    OnLookAtInteractableChanged?.Invoke(null);
                }
            }
        }
        else
        {
            if (_currentLookAtInteractable != null)
            {
                _currentLookAtInteractable.IsSelected = false;
                _currentLookAtInteractable = null;
                OnLookAtInteractableChanged?.Invoke(null);

            }
        }
    }
}
