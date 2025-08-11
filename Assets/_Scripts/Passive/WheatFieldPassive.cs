using Stage.Building;
using UnityEngine;
using Zenject;

public class WheatFieldPassive : Passive
{
    private int Range => PassiveTable.range;
    private float EffectValue => PassiveTable.effectValue;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = SpawnerGridManager.GetNeighbors(spawner, Range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
        }
    }

    public override void Activate(Spawner spawner)
    {
        if (spawner.Building.PassiveTargetGroupType != BuildingGroupType.WheatField)
            return;
        CastSpawner.Building.UpdatePerformance(EffectValue);
        spawner.Building.UpdatePerformance(EffectValue);
    }
}