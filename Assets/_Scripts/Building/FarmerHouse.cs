using Stage.Building;
using Zenject;

public class FarmerHouse : Building
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    private int VillagerPerWheatField => (int)Table.effectValue;

    protected override void PerformAction(IChanger changer)
    {
        int wheatFieldCount = 0;
        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.IsEmpty) continue;
            if (neighbor.Building.GroupType == BuildingGroupType.WheatField)
            {
                wheatFieldCount++;
            }
        }

        BuildingFunction.PerformAction(new ActionContext(wheatFieldCount * VillagerPerWheatField));
    }
}