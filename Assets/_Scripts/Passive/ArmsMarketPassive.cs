using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ArmsMarketPassive : Passive
{
    private int Range => PassiveTable.range;
    private int TargetCount => PassiveTable.effectValue;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = SpawnerGridManager.GetNeighbors(spawner, Range);
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
            spawner.Building.UpdatePerformance(TargetCount);
        }
    }
}