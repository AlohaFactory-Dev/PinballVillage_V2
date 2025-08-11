using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CastlePassive : Passive
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    [SerializeField] private int range = 2;
    [SerializeField] private int addVillageCount = 1;

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
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.House)
        {
            spawner.Building.UpdatePerformance(addVillageCount);
        }
    }
}