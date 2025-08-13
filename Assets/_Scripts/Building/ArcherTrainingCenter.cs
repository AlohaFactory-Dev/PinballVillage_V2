public class ArcherTrainingCenter : Building
{
    private int ArcherCount => (int)Table.effectValue;


    protected override void PerformAction(IChanger changer)
    {
        BuildingFunction.PerformAction(new ActionContext(ArcherCount));
    }
}