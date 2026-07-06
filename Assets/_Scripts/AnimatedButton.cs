using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent OnPointerClickEvent;
    [SerializeField] private UnityEvent OnPointerEnterEvent;
    [SerializeField] private UnityEvent OnPointerExitEvent;

    [SerializeField] private SoundData _onClickSound;
    [SerializeField] private SoundData _onEnterSound;
    [SerializeField] private SoundData _onExitSound;

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

        if (_onClickSound != null)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(_onClickSound)
                .WithRandomPitch()
                .Play();
        }

        OnPointerClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(_originalScale * 1.25f, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);

        if (_onEnterSound != null)
        {
            SoundManager.Instance.CreateSound()
            .WithSoundData(_onEnterSound)
            .WithRandomPitch()
            .Play();
        }

        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();

        transform.DOScale(_originalScale, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);

        if (_onExitSound != null)
        {
            SoundManager.Instance.CreateSound()
            .WithSoundData(_onExitSound)
            .WithRandomPitch()
            .Play();
        }
        

        OnPointerExitEvent?.Invoke();
    }
}
