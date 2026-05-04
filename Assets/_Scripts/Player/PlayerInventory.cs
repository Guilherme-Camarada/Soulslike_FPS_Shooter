using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event Action<Equippable, Equippable> OnCurrentEquippableChanged;

    [Header("References")]
    [SerializeField] private GameInput _gameInput;

    [SerializeField] private List<Equippable> _startingEquippables;
    private List<Equippable> _equippableList;
    [SerializeField] private int _maxEquippables = 4;

    private Equippable _currentEquippable;
    public Equippable CurrentEquippable => _currentEquippable;
    private int _currentEquippableIndex = 0;

    [SerializeField] private Transform _oneHandedSlotTransform;
    [SerializeField] private Transform _twoHandedSlotTransform;
    public Transform OneHandedSlotTransform => _oneHandedSlotTransform;
    public Transform TwoHandedSlotTransform => _twoHandedSlotTransform;

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
        _gameInput.OnUseStartAction -= GameInput_OnUseStartAction;
        _gameInput.OnUseCancelAction -= GameInput_OnUseCancelAction;
    }

    private void Awake()
    {
        _equippableList = new List<Equippable>(_maxEquippables);
    }

    private void Start()
    {
        SpawnEquippables();
        if (_equippableList.Count > 0)
        {
           _currentEquippable = _equippableList[0];
           _currentEquippable.gameObject.SetActive(true);
        }
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

    public void CycleEquippable (int amount)
    {
        if (_equippableList.Count == 0 
            || amount == -1 && _currentEquippableIndex == 0 
            || amount == 1 && _currentEquippableIndex >= _equippableList.Count - 1)
        {
            return;
        }

        _currentEquippableIndex += amount;
        _currentEquippableIndex = Mathf.Clamp(_currentEquippableIndex, 0, _maxEquippables - 1);
        
        if (_equippableList.Contains(_equippableList[_currentEquippableIndex]))
        {
            _currentEquippable.gameObject.SetActive(false);
            Equippable previousEquippable = _currentEquippable;
            _currentEquippable = _equippableList[_currentEquippableIndex];
            _currentEquippable.gameObject.SetActive(true);
            OnCurrentEquippableChanged?.Invoke(previousEquippable, _currentEquippable);
        }
    }

    public void AddEquippable(Equippable equippable)
    {
        if (_equippableList.Count >= _maxEquippables)
        {
            return;
        }

        _equippableList.Add(equippable);     
        equippable.PlayerInventory = this;
    }

    public void RemoveEquippable(Equippable equippable)
    {
        if (_equippableList.Contains(equippable))
        {
            _equippableList.Remove(equippable);
        }
        equippable.PlayerInventory = null;
    }

    public void AssignEquippable(Equippable equippable)
    {
        Transform equipTargetTransform = null;

        if (equippable.EquippableType == EquippableType.OneHanded)
        {
            equipTargetTransform = _oneHandedSlotTransform;
        }
        else if (equippable.EquippableType == EquippableType.TwoHanded)
        {
            equipTargetTransform = _twoHandedSlotTransform;
        }

        equippable.transform.SetParent(equipTargetTransform);

        equippable.transform.localPosition = Vector3.zero;
        equippable.transform.localRotation = Quaternion.identity;
    }

    private void SpawnEquippables()
    {
        foreach (Equippable equippable in _startingEquippables)
        {
            if (equippable != null)
            {
                Equippable spawnedEquippable = Instantiate(equippable, transform.position, Quaternion.identity);
                AddEquippable(spawnedEquippable);
                AssignEquippable(spawnedEquippable);
                spawnedEquippable.gameObject.SetActive(false);
            }
        }
    }


}
