using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CastlePassive : Passive
{
    private int Range => PassiveTable.range;
    private int AddVillageCount => PassiveTable.effectValue;

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
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.House)
        {
            spawner.Building.UpdatePerformance(AddVillageCount);
        }
    }
}