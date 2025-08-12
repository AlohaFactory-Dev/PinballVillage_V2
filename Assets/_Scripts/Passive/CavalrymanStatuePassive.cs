using Stage.Building;
using UnityEngine;
using Zenject;


public class CavalrymanStatuePassive : Passive
{
    private VillagerTracker _villagerTracker;
    private int Range => PassiveTable.range;

    protected override void InjectPassive(Spawner spawner)
    {
        _villagerTracker = spawner.Building.GetComponentInChildren<VillagerTracker>();
        InjectedPassiveSpawners = SpawnerGridManager.GetNeighbors(spawner, Range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
            _villagerTracker.CreateCrossColliders(neighbor.transform.position);
        }

        _villagerTracker.Init(spawner,
            AddSpeed,
            DecreaseSpeed);
    }


    private void AddSpeed(Villager villager)
    {
        villager.VillagerMoveSystem.AdjustSpeed(UpgradeValue);
    }

    private void DecreaseSpeed(Villager villager)
    {
        villager.VillagerMoveSystem.AdjustSpeed(-UpgradeValue);
    }
}