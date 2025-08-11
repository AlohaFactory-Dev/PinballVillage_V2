using Stage.Building;
using UnityEngine;
using Zenject;


public class CavalrymanStatuePassive : Passive
{
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [SerializeField] VillagerTracker villagerTracker;
    [SerializeField] private int range = 3;
    [SerializeField] private int addSpeedValue = 3;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = _spawnerGridManager.GetNeighbors(spawner, range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
            villagerTracker.CreateCrossColliders(neighbor.transform.position);
        }

        villagerTracker.Init(spawner,
            AddSpeed,
            DecreaseSpeed);
    }


    private void AddSpeed(Villager villager)
    {
        villager.VillagerMoveSystem.AdjustSpeed(addSpeedValue);
    }

    private void DecreaseSpeed(Villager villager)
    {
        villager.VillagerMoveSystem.AdjustSpeed(-addSpeedValue);
    }

    public override void Activate(Spawner spawner)
    {
    }
}