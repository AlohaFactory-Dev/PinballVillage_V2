using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class HousePassive : Passive
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    [SerializeField] private int range = 1;
    [SerializeField] private int addVillageCount = 1;

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
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.House)
        {
            spawner.Building.UpdatePerformance(addVillageCount);
        }
    }
}