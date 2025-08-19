using System.Collections;
using System.Collections.Generic;
using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class VillagerManager
{
    private readonly FactoryManager _factoryManager;
    private readonly SpawnerGridManager _spawnerGridManager;
    private readonly List<Villager> _playerVillagers = new List<Villager>();
    private readonly List<Villager> _enemyVillagers = new List<Villager>();
    public List<Villager> PlayerVillagers => _playerVillagers;
    public List<Villager> EnemyVillagers => _enemyVillagers;
    public LordCharacter LordCharacter { get; private set; }

    public VillagerManager(FactoryManager factoryManager, SpawnerGridManager spawnerGridManager)
    {
        _factoryManager = factoryManager;
        _spawnerGridManager = spawnerGridManager;
    }

    public Villager SpawnVillager(Spawner spawner, VillagerType villagerType)
    {
        var table = TableListContainer.Get<VillagerTableList>().GetVillagerTable(villagerType, spawner.CurrentOwner);
        var villager = _factoryManager.VillagerFactory.GetVillager(table.id);

        var nearestSpawner = _spawnerGridManager.GetNearestEmptySpawner(spawner, spawner.CurrentOwner);
        villager.Init(nearestSpawner.transform.position, table);
        if (spawner.CurrentOwner == OwnerType.Enemy)
        {
            _enemyVillagers.Add(villager);
        }
        else if (spawner.CurrentOwner == OwnerType.Player)
        {
            _playerVillagers.Add(villager);
            if (VillagerType.Lord == villagerType)
            {
                LordCharacter = villager as LordCharacter;
            }
        }

        return villager;
    }

    public void RemoveVillager(Villager villager)
    {
        if (_playerVillagers.Contains(villager))
        {
            _playerVillagers.Remove(villager);
            villager.Release();
        }
        else if (_enemyVillagers.Contains(villager))
        {
            _enemyVillagers.Remove(villager);
            villager.Release();
        }
    }
}