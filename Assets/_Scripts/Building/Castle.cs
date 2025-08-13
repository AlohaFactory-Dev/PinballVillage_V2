using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class Castle : Building
{
    [Inject] private VillagerManager _villagerManager;
    private int GoldAmount => (int)Table.effectValue;

    public override void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        if (OwnerType == OwnerType.Player && !isLevelUp)
            _villagerManager.SpawnVillager(Spawner, VillagerType.Lord);
    }

    protected override void PerformAction(IChanger changer)
    {
        BuildingFunction.PerformAction(new ActionContext(changer, GoldAmount));
    }
}