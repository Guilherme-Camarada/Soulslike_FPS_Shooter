using DG.Tweening;
using System;
using UnityEngine;

public class DamageableFX : MonoBehaviour
{
    private Damageable _damageable;

    [Header("Audio")]
    [SerializeField] private SoundData _deathSoundData;
    [SerializeField] private SoundData _hitSoundData;

    [Header("Visuals")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashDuration = 0.15f;

    [SerializeField] private string _customColorPropertyOverride = "";

    private string _activeColorProperty;
    private Color _originalColor;
    private Tween _flashTween;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnDamageTakenAction += Damageable_OnDamageTakenAction;
        _damageable.OnDeathAction += Damageable_OnDeathAction;
    }

    private void Damageable_OnDeathAction(Damageable obj)
    {
        if (_deathSoundData != null)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(_deathSoundData)
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play();
        }

        _flashTween?.Kill();
    }

    private void Damageable_OnDamageTakenAction()
    {
        if (_hitSoundData != null)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(_hitSoundData)
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play();
        }

        if (_renderer != null)
        {
            _flashTween?.Kill();

            _renderer.material.SetColor(_activeColorProperty, _flashColor);

            _flashTween = _renderer.material.DOColor(_originalColor, _activeColorProperty, _flashDuration).SetLink(gameObject);
        }
    }

    private void OnDisable()
    {
        _damageable.OnDamageTakenAction -= Damageable_OnDamageTakenAction;
        _damageable.OnDeathAction -= Damageable_OnDeathAction;
    }

    private void Start()
    {
        if (_renderer != null)
        {
            DetermineColorProperty();
            _originalColor = _renderer.material.GetColor(_activeColorProperty);
        }
    }

    private void DetermineColorProperty()
    {
        if (!string.IsNullOrEmpty(_customColorPropertyOverride))
        {
            _activeColorProperty = _customColorPropertyOverride;
            return;
        }

        if (_renderer.material.HasProperty("_BaseColor"))
        {
            _activeColorProperty = "_BaseColor";
        }
        else if (_renderer.material.HasProperty("_Color"))
        {
            _activeColorProperty = "_Color";
        }
        else
        {
            Debug.LogWarning($"[DamageableFX] Could not find a color property on {_renderer.gameObject.name}'s material!");
            _activeColorProperty = "_Color";
        }
    }
}
