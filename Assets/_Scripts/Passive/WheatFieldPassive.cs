using Stage.Building;
using UnityEngine;
using Zenject;

public class WheatFieldPassive : Passive
{
    [SerializeField] int range = 1;
    [SerializeField] float effectValue = 1f;
    [Inject] SpawnerGridManager _spawnerGridManager;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = _spawnerGridManager.GetNeighbors(spawner, range);
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
        }
    }

    public override void Activate(Spawner spawner)
    {
        if (spawner.Building.PassiveTargetGroupType != BuildingGroupType.WheatField)
            return;
        CastSpawner.Building.UpdatePerformance(effectValue);
        spawner.Building.UpdatePerformance(effectValue);
    }
}