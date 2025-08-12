using Stage.Building;
using UnityEngine;
using Zenject;


public class ArcherStatuePassive : Passive
{
    private VillagerTracker _villagerTracker;
    private int Range => PassiveTable.range;
    private int TargetCount => PassiveTable.effectValue;

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


    public override void Activate(Spawner spawner)
    {
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.Tower)
        {
            spawner.Building.UpdatePerformance(1);
        }
    }

    private void AddAttack(Villager villager)
    {
        if (villager.VillagerType == VillagerType.Archer)
        {
            (villager as ArcherCharacter).AddAttackCount(TargetCount);
        }
    }

    private void DecreaseAttack(Villager villager)
    {
        if (villager.VillagerType == VillagerType.Archer)
        {
            (villager as ArcherCharacter).DecreaseAttackCount(TargetCount);
        }
    }
}