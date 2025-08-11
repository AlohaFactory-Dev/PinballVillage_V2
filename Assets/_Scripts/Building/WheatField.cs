using Stage.Building;
using Zenject;

public class WheatField : Building
{
    [Inject] SpawnerGridManager _spawnerGridManager;

    public override void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        Collider2D.isTrigger = true;
    }

    protected override void PerformAction(IChanger changer = null, int value = 0, CalculateType calculate = CalculateType.Add)
    {
        if (Spawner.CurrentOwner == OwnerType.Player)
        {
            var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);
            int wheatFieldCount = 0;
            foreach (var neighbor in neighbors)
            {
                if (neighbor.CurrentOwner == OwnerType.Player)
                {
                    if (!neighbor.IsEmpty)
                    {
                        if (neighbor.Building.PassiveTargetGroupType == BuildingGroupType.WheatField)
                        {
                            wheatFieldCount++;
                        }
                    }
                }
            }

            base.PerformAction(changer, wheatFieldCount * (int)Table.values[0], calculate);
        }
    }
}