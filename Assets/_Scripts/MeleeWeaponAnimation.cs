using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
public class MeleeWeaponAnimation : MonoBehaviour
{
    private MeleeWeapon _meleeWeapon;

    [Header("Swing Timings")]
    [SerializeField] private float _attackSpeedMultiplier = 1f;
    [SerializeField] private float _windupTime = 0.15f;
    [SerializeField] private float _strikeTime = 0.20f;
    [SerializeField] private float _recoveryTime = 0.25f;

    [Header("Phase 1: Windup")]
    [SerializeField] private Vector3 _windupRotation = new Vector3(0f, 0f, -90f);
    [SerializeField] private Vector3 _windupPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Ease _windupEase = Ease.InOutSine;

    [Header("Phase 2: The Strike & Slide")]
    [SerializeField] private Vector3 _strikeRotation = new Vector3(0f, -120f, -90f);
    [SerializeField] private Vector3 _strikePosition = new Vector3(-0.2f, 0f, 0f);
    [SerializeField] private Ease _strikeEase = Ease.InOutSine;
    [SerializeField] private Ease _returnEase = Ease.InOutSine;

    private Vector3 _startPosition;
    private Vector3 _startRotationEuler;
    private bool _isSwinging = false;

    private void Awake()
    {
        _meleeWeapon = GetComponent<MeleeWeapon>();
    }

    private void OnEnable()
    {
        _meleeWeapon.OnWeaponAttack += MeleeWeapon_OnWeaponAttack;
    }

    private void MeleeWeapon_OnWeaponAttack()
    {
        Swing();
    }

    private void OnDisable()
    {
        _meleeWeapon.OnWeaponAttack -= MeleeWeapon_OnWeaponAttack;
    }

    private void Start()
    {
        _startPosition = transform.localPosition;
        _startRotationEuler = transform.localEulerAngles;
    }

    public void Swing()
    {
        if (!_isSwinging)
        {
            _isSwinging = true;
            PlaySwingAnimation();
        }
    }

    private void PlaySwingAnimation()
    {
        float tWindup = _windupTime / _attackSpeedMultiplier;
        float tStrike = _strikeTime / _attackSpeedMultiplier;
        float tRecovery = _recoveryTime / _attackSpeedMultiplier;

        Sequence swingSequence = DOTween.Sequence();

        swingSequence.Append(transform.DOLocalMove(_windupPosition, tWindup).SetEase(_windupEase));
        swingSequence.Join(transform.DOLocalRotate(_windupRotation, tWindup).SetEase(_windupEase));

        swingSequence.Append(transform.DOLocalMove(_strikePosition, tStrike).SetEase(_strikeEase));
        swingSequence.Join(transform.DOLocalRotate(_strikeRotation, tStrike).SetEase(_strikeEase));

        swingSequence.Append(transform.DOLocalMove(_startPosition, tRecovery).SetEase(_returnEase));
        swingSequence.Join(transform.DOLocalRotate(_startRotationEuler, tRecovery).SetEase(_returnEase));

        swingSequence.OnComplete(() =>
        {
            _isSwinging = false;
        });
    }
}
