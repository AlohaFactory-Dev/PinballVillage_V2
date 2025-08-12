public class ArcherTrainingCenter : Building
{
    private int ArcherCount => (int)Table.effectValue;

    protected override void OnSpawnPerformAction()
    {
        PerformAction(new ActionContext(ArcherCount));
    }
}