public class VillagerFactory : FactorySystem<Villager, string>
{
    protected override string LabelId => "Villager";

    protected override string TranslateStringKeyToID(string primaryKey)
    {
        return primaryKey;
    }

    public Villager GetVillager(string villagerId)
    {
        tempObject = GetObject(villagerId);
        return tempObject;
    }
}