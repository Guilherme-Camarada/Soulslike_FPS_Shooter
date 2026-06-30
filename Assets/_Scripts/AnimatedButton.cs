using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent OnPointerClickEvent;
    [SerializeField] private UnityEvent OnPointerEnterEvent;
    [SerializeField] private UnityEvent OnPointerExitEvent;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(_originalScale * 0.9f, 0.1f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale * 1.25f, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            });

        OnPointerClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(_originalScale * 1.25f, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);

        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(_originalScale, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);

        OnPointerExitEvent?.Invoke();
    }
}
