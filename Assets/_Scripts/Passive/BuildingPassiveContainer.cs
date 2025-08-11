using UnityEngine;

// Castle_3
// Bank_1
// House_3
// ArmsMarket
// CavalrymanStatue
// ArcherStatue
public static class BuildingPassiveContainer
{
    public static Passive GetPassive(string id)
    {
        switch (id)
        {
            case "Castle_3":
                return new CastlePassive();
            case "Bank_1":
                return new BankPassive();
            case "House_3":
                return new HousePassive();
            case "ArmsMarket":
                return new ArmsMarketPassive();
            case "CavalrymanStatue":
                return new CavalrymanStatuePassive();
            case "ArcherStatue":
                return new ArcherStatuePassive();
            default:
                Debug.LogError($"Passive with id {id} not found.");
                return null;
        }
    }
}