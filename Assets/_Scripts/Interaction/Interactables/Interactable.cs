using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public event Action<PlayerInteractor> OnInteractAction;
    public event Action<bool> OnInteractableHoverAction;

    public abstract void Interact(Interactable interactable, PlayerInteractor playerInteractor);
    public abstract void InteractSecondary(Interactable interactable, PlayerInteractor playerInteractor);

    public void ChangeHover(bool isHovered)
    {
        OnInteractableHoverAction?.Invoke(isHovered);
    }
}


