public class EnemyCastle : Building
{
    private int VillagerCount => (int)Table.effectValue;

    protected override void PerformAction(IChanger changer)
    {
        BuildingFunction.PerformAction(new ActionContext(VillagerCount));
    }
}