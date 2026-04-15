using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Equippable : MonoBehaviour
{
    public event Action OnUseAction;

    [Header("References")]
    protected PlayerInventory _playerInventory;
    public PlayerInventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }

    [SerializeField] protected EquippableType _equippableType;
    public EquippableType EquippableType => _equippableType;

    public abstract void UseStart();
    public abstract void UseStop();

    public bool IsEquipped()
    {
        return _playerInventory != null;
    }
}

public enum EquippableType
{
    OneHanded,
    TwoHanded
}