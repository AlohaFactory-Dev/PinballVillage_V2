public class Market : Building
{
    private int GoldAmount => (int)Table.effectValue;

    protected override void PerformAction(IChanger changer)
    {
        BuildingFunction.PerformAction(new ActionContext(changer, GoldAmount));
    }
}