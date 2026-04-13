using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [Header("References")]
    private RangedWeapon _rangedWeapon;
    private Equippable _equippable;

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

    private void Awake()
    {
        _rangedWeapon = GetComponent<RangedWeapon>();
        _equippable = GetComponent<Equippable>();   
    }

    private void Start()
    {
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!_equippable.IsEquipped())
        {
            return;
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

    private void RangedWeapon_OnReloadAction(float cooldownTimer)
    {
        
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
