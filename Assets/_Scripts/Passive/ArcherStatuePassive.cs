using Stage.Building;
using UnityEngine;
using Zenject;


public class ArcherStatuePassive : Passive
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
            AddAttack,
            DecreaseAttack);
    }


    private void AddAttack(Villager villager)
    {
        if (villager.VillagerType == VillagerType.Archer)
        {
            (villager as ArcherCharacter).AddAttackCount(UpgradeValue);
        }
    }

    private void DecreaseAttack(Villager villager)
    {
        if (villager.VillagerType == VillagerType.Archer)
        {
            (villager as ArcherCharacter).DecreaseAttackCount(UpgradeValue);
        }
    }
}