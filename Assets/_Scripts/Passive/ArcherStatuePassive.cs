using Stage.Building;
using UnityEngine;
using Zenject;


public class ArcherStatuePassive : Passive
{
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [SerializeField] VillagerTracker villagerTracker;
    [SerializeField] private int range = 3;
    [SerializeField] private int targetCount = 1;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = _spawnerGridManager.GetNeighbors(spawner, range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
            villagerTracker.CreateCrossColliders(neighbor.transform.position);
        }

        villagerTracker.Init(spawner,
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
            (villager as ArcherCharacter).AddAttackCount(targetCount);
        }
    }

    private void DecreaseAttack(Villager villager)
    {
        if (villager.VillagerType == VillagerType.Archer)
        {
            (villager as ArcherCharacter).DecreaseAttackCount(targetCount);
        }
    }
}