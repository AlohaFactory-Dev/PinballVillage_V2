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

    protected override void OnCollisionPerformAction(IChanger changer)
    {
        // Whidmill은 wheatCount에 따라 성능을 조정함.
        PerformAction(new ActionContext(changer, GoldAmount));
    }
}