using UnityEngine;

public class PlayerInventory : UnitInventory
{
    [Header("References")]
    [SerializeField] private GameInput _gameInput;

    private void OnEnable()
    {
        _gameInput.OnUseStartAction += GameInput_OnUseStartAction;
        _gameInput.OnUseCancelAction += GameInput_OnUseCancelAction;
        _gameInput.OnReloadAction += GameInput_OnReloadAction;
    }

    private void GameInput_OnReloadAction()
    {
        if (_currentEquippable.TryGetComponent(out RangedWeapon rangedWeapon))
        {
            rangedWeapon.Reload();
        }
    }

    private void GameInput_OnUseCancelAction()
    {
        if (_currentEquippable != null)
        {
            _currentEquippable.UseStop();
        }

    }

    private void GameInput_OnUseStartAction()
    {
        if (_currentEquippable != null)
        {
            _currentEquippable.UseStart();
        }
    }

    private void OnDisable()
    {
        _gameInput.OnReloadAction -= GameInput_OnReloadAction;
        _gameInput.OnUseStartAction -= GameInput_OnUseStartAction;
        _gameInput.OnUseCancelAction -= GameInput_OnUseCancelAction;
    }

    private void Update()
    {
        Vector2 mouseScrollVector = _gameInput.GetMouseScrollInputVector();

        if (mouseScrollVector.y > 0f)
        {
            CycleEquippable(1);
        }
        else if (mouseScrollVector.y < 0f)
        {
            CycleEquippable(-1);
        }
    }


}
