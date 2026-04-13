using System.Collections.Generic;
using UnityEngine;

public class HoveredInteractableVisual : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;

    [SerializeField] private List<GameObject> _highlightedVisualList;
    [SerializeField] private RenderingLayerMask _highlightLayerMask;


    private void OnEnable()
    {
        _interactable.OnInteractableHoverAction += Interactable_OnHighlightChanged;
    }

    private void OnDisable()
    {
        _interactable.OnInteractableHoverAction -= Interactable_OnHighlightChanged;
    }

    private void Interactable_OnHighlightChanged(bool obj)
    {
        if (obj)
        {
           EnableVisuals();

        }
        else
        {
            DisableVisuals();
        }
    }

    private void EnableVisuals()
    {
        foreach (GameObject highlightedVisual in _highlightedVisualList)
        {
            Renderer renderer = highlightedVisual.GetComponent<Renderer>();
            renderer.renderingLayerMask |= _highlightLayerMask;
        }
    }

    private void DisableVisuals()
    {
        foreach (GameObject highlightedVisual in _highlightedVisualList)
        {
            Renderer renderer = highlightedVisual.GetComponent<Renderer>();
            renderer.renderingLayerMask &= ~(uint)_highlightLayerMask;
        }
    }

}
