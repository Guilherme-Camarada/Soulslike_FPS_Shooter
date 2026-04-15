using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameInput _gameInput;
    private InteractableDetector _interactableDetector;

    private Interactable _currentLookAtInteractable;

    private void Awake()
    {
        _interactableDetector = GetComponent<InteractableDetector>();
    }

    private void OnEnable()
    {
        _interactableDetector.OnLookAtInteractableChanged += InteractableDetector_OnLookAtInteractableChanged;

        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }


    private void OnDisable()
    {
        _interactableDetector.OnLookAtInteractableChanged -= InteractableDetector_OnLookAtInteractableChanged;

        _gameInput.OnInteractAction -= GameInput_OnInteractAction;

    }

    private void InteractableDetector_OnLookAtInteractableChanged(Interactable obj)
    {
        _currentLookAtInteractable = obj;
    }

    private void GameInput_OnInteractAction()
    {
        if (_currentLookAtInteractable != null)
        {
            _currentLookAtInteractable.Interact(_currentLookAtInteractable, this);
        }
        //else if (_currentLookAtInteractable == null)
        //{
        //    _equippableParent.CurrentEquippable.InteractSecondary(_equippableParent.CurrentEquippable, this);
        //}
    }
}
