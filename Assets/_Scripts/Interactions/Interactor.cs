using System;
using UnityEngine;

[RequireComponent(typeof(InteractableDetector))]
public class Interactor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameInput _gameInput;
    private InteractableDetector _interactableDetector;

    private Interactable _currentInteractable;

    private void Awake()
    {
        _interactableDetector = GetComponent<InteractableDetector>();
    }

    private void OnEnable()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _interactableDetector.OnLookAtInteractableChanged += InteractableDetector_OnLookAtInteractableChanged;
    }

    private void InteractableDetector_OnLookAtInteractableChanged(Interactable interactable)
    {
        _currentInteractable = interactable;
    }

    private void GameInput_OnInteractAction()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact(this);
        }
    }

    private void OnDisable()
    {
        _gameInput.OnInteractAction -= GameInput_OnInteractAction;
        _interactableDetector.OnLookAtInteractableChanged -= InteractableDetector_OnLookAtInteractableChanged;
    }

    
}
