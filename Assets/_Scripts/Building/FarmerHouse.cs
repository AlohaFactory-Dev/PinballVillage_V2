using Stage.Building;
using Zenject;

public class FarmerHouse : Building
{
    [Inject] SpawnerGridManager _spawnerGridManager;

    protected override void PerformAction(IChanger changer = null, int value = 0, CalculateType calculate = CalculateType.Add)
    {
        int wheatFieldCount = 0;
        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.IsEmpty) continue;
            if (neighbor.Building.PassiveTargetGroupType == BuildingGroupType.WheatField)
            {
                wheatFieldCount++;
            }
        }

        base.PerformAction(changer, wheatFieldCount, CalculateType.Multiply);
    }
}