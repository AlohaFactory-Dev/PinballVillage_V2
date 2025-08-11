using Stage.Building;

namespace FactorySystem
{
    public class BuildingFactory : FactorySystem<Building, string>
    {
        protected override string LabelId => "Building";

        protected override string TranslateStringKeyToID(string primaryKey)
        {
            return primaryKey;
        }

        public Building GetBuilding(string buildingId)
        {
            tempObject = GetObject(buildingId);
            return tempObject;
        }
    }
}