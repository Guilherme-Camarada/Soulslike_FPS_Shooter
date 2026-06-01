using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public event Action<bool> OnInteractableSelectedAction;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnInteractableSelectedAction?.Invoke(_isSelected);
            }
        }
    }

    public abstract void Interact(Interactor playerInteractor);
}
