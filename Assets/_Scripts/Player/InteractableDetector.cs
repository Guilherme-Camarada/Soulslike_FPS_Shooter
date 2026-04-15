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

    void Update()
    {
        HandlePlayerInteractions(_interactionRange);
    }

    private void HandlePlayerInteractions(float interactionRange)
    {
        if (Physics.Raycast(_cameraLookAt.position, _cameraLookAt.forward, out RaycastHit hitInfo, interactionRange, _interactionLayerMask))
        {
            if (hitInfo.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                if (_currentLookAtInteractable != null && _currentLookAtInteractable != interactable)
                {
                    _currentLookAtInteractable.ChangeHover(false);
                }
                _currentLookAtInteractable = interactable;
                OnLookAtInteractableChanged?.Invoke(interactable);
                _currentLookAtInteractable.ChangeHover(true);
            }
            else
            {
                if (_currentLookAtInteractable != null)
                {
                    _currentLookAtInteractable.ChangeHover(false);
                    _currentLookAtInteractable = null;
                    OnLookAtInteractableChanged?.Invoke(null);
                }
            }
        }
        else
        {
            if (_currentLookAtInteractable != null)
            {
                _currentLookAtInteractable.ChangeHover(false);
                _currentLookAtInteractable = null;
                OnLookAtInteractableChanged?.Invoke(null);

            }
        }
    }


}
