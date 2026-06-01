using DG.Tweening;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [Header("References")]
    private ShootUsable _rangedWeapon;

    [Header("Recoil Animation Settings")]
    [SerializeField] private Vector3 _kickbackAmount = new Vector3(0f, 0f, -0.2f);
    [SerializeField] private float _maxKickbackDistance = -0.25f;
    [SerializeField] private float _snappiness = 15f;
    [SerializeField] private float _recoilRecoverySpeed = 5f;

    [Header("Reload Animation Settings")]
    [SerializeField] private Vector3 _reloadPositionDip = new Vector3(0f, -0.4f, 0f);
    [SerializeField] private Vector3 _reloadRotationTilt = new Vector3(30f, 15f, -15f);

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private Vector3 _targetPosition;
    private Vector3 _currentPosition;

    private Vector3 _reloadPositionOffset;
    private Vector3 _reloadRotationOffset;

    private Sequence _reloadSequence;

    private void Awake()
    {
        _rangedWeapon = GetComponent<ShootUsable>();
    }

    private void Start()
    {
        _initialPosition = Vector3.zero;
        _initialRotation = Quaternion.identity;

        _targetPosition = _initialPosition;
        _currentPosition = _initialPosition;
    }

    private void Update()
    {
        if (_rangedWeapon.TryGetComponent(out EquipInteractable equippable))
        {
            if (!equippable.IsEquipped || equippable.IsAnimatingEquip) return;
        }

        _targetPosition = Vector3.Lerp(_targetPosition, _initialPosition, _recoilRecoverySpeed * Time.deltaTime);
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, _snappiness * Time.deltaTime);

        transform.localPosition = _currentPosition + _reloadPositionOffset;
        transform.localRotation = _initialRotation * Quaternion.Euler(_reloadRotationOffset);
    }

    private void OnEnable()
    {
        _rangedWeapon.OnFireAction += RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction += RangedWeapon_OnReloadAction; 
    }

    private void HandleEquipStateAction(bool isEquipped)
    {
        if (!isEquipped)
        {
            _reloadPositionOffset = Vector3.zero;
            _reloadRotationOffset = Vector3.zero;

            if (_reloadSequence != null && _reloadSequence.IsActive())
            {
                _reloadSequence.Kill();
            }

            _targetPosition = Vector3.zero;
            _currentPosition = Vector3.zero;

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        } else
        {
            _targetPosition = _initialPosition;
            _currentPosition = _initialPosition;
        }
    }

    private void RangedWeapon_OnReloadAction(float cooldownTimer)
    {
        _reloadSequence?.Kill();
        _reloadSequence = DOTween.Sequence();

        float dipTime = cooldownTimer * 0.3f;
        _reloadSequence.Append(DOTween.To(() => _reloadPositionOffset, x => _reloadPositionOffset = x, _reloadPositionDip, dipTime).SetEase(Ease.InOutSine));
        _reloadSequence.Join(DOTween.To(() => _reloadRotationOffset, x => _reloadRotationOffset = x, _reloadRotationTilt, dipTime).SetEase(Ease.InOutSine));

        _reloadSequence.AppendInterval(cooldownTimer * 0.4f);

        float recoverTime = cooldownTimer * 0.3f;
        _reloadSequence.Append(DOTween.To(() => _reloadPositionOffset, x => _reloadPositionOffset = x, Vector3.zero, recoverTime).SetEase(Ease.OutBack));
        _reloadSequence.Join(DOTween.To(() => _reloadRotationOffset, x => _reloadRotationOffset = x, Vector3.zero, recoverTime).SetEase(Ease.OutBack));
    }

    private void OnDisable()
    {
        _rangedWeapon.OnFireAction -= RangedWeapon_OnFireAction;
        _rangedWeapon.OnReloadAction -= RangedWeapon_OnReloadAction;
    }

    private void RangedWeapon_OnFireAction()
    {
        ApplyKickback();
    }

    public void ApplyKickback()
    {
        float randomX = Random.Range(-0.02f, 0.02f);
        float randomY = Random.Range(-0.02f, 0.02f);

        _targetPosition += new Vector3(randomX, randomY, _kickbackAmount.z);
        _targetPosition.z = Mathf.Clamp(_targetPosition.z, _maxKickbackDistance, 0f);
    }
}
