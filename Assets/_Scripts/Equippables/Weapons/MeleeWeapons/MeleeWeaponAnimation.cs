using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
public class MeleeWeaponAnimation : MonoBehaviour
{
    private MeleeWeapon _meleeWeapon;

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
        PlaySwingAnimation();
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

    private void PlaySwingAnimation()
    {
        float tWindup = _meleeWeapon.WindupTime / _meleeWeapon.AttackSpeedMultiplier;
        float tStrike = _meleeWeapon.StrikeTime / _meleeWeapon.AttackSpeedMultiplier;
        float tRecovery = _meleeWeapon.RecoveryTime / _meleeWeapon.AttackSpeedMultiplier;

        Sequence swingSequence = DOTween.Sequence();

        swingSequence.Append(transform.DOLocalMove(_windupPosition, tWindup).SetEase(_windupEase));
        swingSequence.Join(transform.DOLocalRotate(_windupRotation, tWindup).SetEase(_windupEase));

        swingSequence.Append(transform.DOLocalMove(_strikePosition, tStrike).SetEase(_strikeEase));
        swingSequence.Join(transform.DOLocalRotate(_strikeRotation, tStrike).SetEase(_strikeEase));

        swingSequence.Append(transform.DOLocalMove(_startPosition, tRecovery).SetEase(_returnEase));
        swingSequence.Join(transform.DOLocalRotate(_startRotationEuler, tRecovery).SetEase(_returnEase));

        swingSequence.OnComplete(() =>
        {
            _meleeWeapon.IsSwinging = false;
        });
    }
}
