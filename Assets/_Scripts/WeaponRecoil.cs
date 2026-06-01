using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    private MouseLook _mouseLook;
    private ShootUsable _weapon;

    [Header("Recoil Settings")]
    [Range(0f, 7f)][SerializeField] private float _recoilAmountY = 3f;
    [Range(0f, 3f)][SerializeField] private float _recoilAmountX = 1f;
    [SerializeField] private float _snappiness = 10f;
    [SerializeField] private float _recoilRecoverySpeed = 2f;

    private void Awake()
    {
        _weapon = GetComponent<ShootUsable>();
    }

    private void Start()
    {
        _mouseLook = MouseLook.Instance;
    }

    private void OnEnable()
    {
        _weapon.OnFireAction += Weapon_OnFireAction;
    }

    private void Update()
    {
        _mouseLook.ApplyRotationModifiers(_snappiness, _recoilRecoverySpeed);
    }

    private void Weapon_OnFireAction()
    {
        CalculateRecoil();   
    }

    private void OnDisable()
    {
        _weapon.OnFireAction -= Weapon_OnFireAction;
    }

    private void CalculateRecoil()
    {
        float randomXRecoil = Random.Range(-_recoilAmountX, _recoilAmountX);

        _mouseLook.ChangeLookTarget(randomXRecoil, _recoilAmountY);
        
    }
}
