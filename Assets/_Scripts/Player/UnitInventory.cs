using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UnitInventory : MonoBehaviour
{
    public event Action<Usable, Usable> OnCurrentEquippableChanged;

    [SerializeField] private List<Usable> _startingEquippables;
    protected List<Usable> _equippableList;
    [SerializeField] private int _maxEquippables = 4;

    protected Usable _currentEquippable;
    public Usable CurrentEquippable => _currentEquippable;
    protected int _currentEquippableIndex = 0;

    [SerializeField] private Transform _oneHandedSlotTransform;
    [SerializeField] private Transform _twoHandedSlotTransform;
    public Transform OneHandedSlotTransform => _oneHandedSlotTransform;
    public Transform TwoHandedSlotTransform => _twoHandedSlotTransform;


    private void Awake()
    {
        _equippableList = new List<Usable>(_maxEquippables);
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
            Usable previousEquippable = _currentEquippable;
            _currentEquippable = _equippableList[_currentEquippableIndex];
            _currentEquippable.gameObject.SetActive(true);
            OnCurrentEquippableChanged?.Invoke(previousEquippable, _currentEquippable);
        }
    }

    public void AddEquippable(Usable equippable)
    {
        if (_equippableList.Count >= _maxEquippables)
        {
            return;
        }

        _equippableList.Add(equippable);     
        
    }

    public void RemoveEquippable(Usable equippable)
    {
        if (_equippableList.Contains(equippable))
        {
            _equippableList.Remove(equippable);
        }
       
    }

    public void AssignEquippable(Usable equippable)
    {
        Transform equipTargetTransform = null;

        equipTargetTransform = _twoHandedSlotTransform;


        equippable.transform.SetParent(equipTargetTransform);

        equippable.transform.localPosition = Vector3.zero;
        equippable.transform.localRotation = Quaternion.identity;
    }

    private void SpawnEquippables()
    {
        foreach (Usable equippable in _startingEquippables)
        {
            if (equippable != null)
            {
                Usable spawnedEquippable = Instantiate(equippable, transform.position, Quaternion.identity);
                if (spawnedEquippable.TryGetComponent(out RangedWeapon rangedWeapon))
                {
                    rangedWeapon.ShootOrigin = Camera.main.transform;
                }

                AddEquippable(spawnedEquippable);
                AssignEquippable(spawnedEquippable);
                spawnedEquippable.gameObject.SetActive(false);
            }
        }
    }


}
