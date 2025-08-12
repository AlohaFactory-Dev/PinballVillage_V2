using Stage.Building;
using Zenject;

public class WheatField : Building
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    private int GoldPerWheatField => (int)Table.values[0];

    public override void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        Collider2D.isTrigger = true;
    }

    protected override void AutoPerformAction()
    {
        if (Spawner.CurrentOwner != OwnerType.Player) return;

        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);
        int wheatFieldCount = 0;
        foreach (var neighbor in neighbors)
        {
            if (neighbor.CurrentOwner == OwnerType.Player)
            {
                if (!neighbor.IsEmpty)
                {
                    if (neighbor.Building.GroupType == BuildingGroupType.WheatField)
                    {
                        wheatFieldCount++;
                    }
                }
            }
        }

        PerformAction(new ActionContext(wheatFieldCount * GoldPerWheatField));
    }
}