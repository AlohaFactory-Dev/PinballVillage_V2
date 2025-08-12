using System.Collections.Generic;
using Aloha.Coconut;
using Stage.Building;
using UnityEngine;
using Zenject;

public class VillagerSpawn : BuildingFunction
{
    [Inject] private VillagerManager _villagerManager;
    [SerializeField] private VillagerType villagerType = VillagerType.Normal;
    private readonly List<Villager> _villagers = new();

    public override void PerformAction(ActionContext actionContext)
    {
        int count = actionContext.Value + UpgradeValue;
        for (int i = 0; i < count; i++)
        {
            _villagers.Add(_villagerManager.SpawnVillager(Spawner, villagerType));
        }

        if (Spawner.CurrentOwner == OwnerType.Player)
        {
            FloatingTextPoint.ShowVillagerSpawnText(count, villagerType);
        }
    }

    public override void UpgradePerformance(Passive passive)
    {
        UpgradeValue += passive.UpgradeValue;
        for (int i = 0; i < UpgradeValue; i++)
            _villagers.Add(_villagerManager.SpawnVillager(Spawner, villagerType));
    }

    public override void DowngradePerformance(Passive passive)
    {
        UpgradeValue -= passive.UpgradeValue;
        if (_villagers.Count > 0)
        {
            int countToRemove = Mathf.Min(passive.UpgradeValue, _villagers.Count);
            for (int i = 0; i < countToRemove; i++)
            {
                var villager = _villagers[^1];
                _villagerManager.RemoveVillager(villager);
                _villagers.Remove(villager);
            }
        }
        else
        {
            Debug.LogWarning("No villagers to remove during downgrade.");
        }
    }

    public override void DestroyAction()
    {
        foreach (var villager in _villagers)
        {
            _villagerManager.RemoveVillager(villager);
        }

        _villagers.Clear();
    }
}