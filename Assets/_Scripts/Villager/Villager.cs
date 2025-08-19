using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Unit;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(RecycleObject))]
public abstract class Villager : MonoBehaviour, IChanger
{
    [SerializeField] private OwnerType ownerType;
    private VillagerMoveSystem _villagerMoveSystem;
    private RecycleObject _recycleObject;
    protected VillagerTable Table { get; private set; }
    public OwnerType OwnerType => ownerType;
    public Transform Transform => transform;
    public abstract VillagerType VillagerType { get; }
    public int AttackPower => Table.attackPower;

    public VillagerMoveSystem VillagerMoveSystem => _villagerMoveSystem;


    public void Init(Vector2 spawnPoint, VillagerTable table, float addedMoveSpeed)
    {
        Table = table;
        transform.position = spawnPoint;
        _recycleObject = GetComponent<RecycleObject>();
        _villagerMoveSystem = GetComponentInChildren<VillagerMoveSystem>();
        _villagerMoveSystem.Init(this, table.moveSpeed + addedMoveSpeed);
    }

    public void Release()
    {
        _recycleObject.Release();
    }

    public virtual void SpawnerChangeAction(Spawner spawner)
    {
    }
}