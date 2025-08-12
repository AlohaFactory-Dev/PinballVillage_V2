public class House : Building
{
    private int VillagerCount => (int)Table.effectValue;

    protected override void OnSpawnPerformAction()
    {
        PerformAction(new ActionContext(VillagerCount));
    }
}