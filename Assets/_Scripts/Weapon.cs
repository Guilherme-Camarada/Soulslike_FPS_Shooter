using UnityEngine;

public abstract class Weapon : EquipInteractable
{
    public abstract void Attack();
    public abstract void StopAttack();
}
