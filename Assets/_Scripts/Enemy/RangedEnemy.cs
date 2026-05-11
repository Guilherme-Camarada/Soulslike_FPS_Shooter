using System;
using UnityEngine;

public class RangedEnemy : Enemy
{
    private EnemyInventory _enemyInventory;
    [SerializeField] private Transform _weaponSlot;

    protected override void Awake()
    {
        base.Awake();
        _enemyInventory = GetComponent<EnemyInventory>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking from range!");
        _enemyInventory.CurrentEquippable.UseStart();
    }

    protected override void StopAttack()
    {
        Debug.Log($"{gameObject.name} stopped attacking from range!");
        _enemyInventory.CurrentEquippable.UseStop();
    }
}
