using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ArmsMarketPassive : Passive
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    [SerializeField] private int range = 3;
    [SerializeField] private int targetCount = 1;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = _spawnerGridManager.GetNeighbors(spawner, range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
            Activate(neighbor);
        }
    }

    public override void Activate(Spawner spawner)
    {
        if (spawner.IsEmpty) return;
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.Tower)
        {
            spawner.Building.UpdatePerformance(targetCount);
        }
    }
}