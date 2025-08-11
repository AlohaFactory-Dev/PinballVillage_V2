using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class Castle : Building
{
    [Inject] private VillagerManager _villagerManager;
    public new Spawner Spawner => base.Spawner;

    public override void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        if (OwnerType == OwnerType.Player && !isLevelUp)
            _villagerManager.SpawnVillager(Spawner, VillagerType.Lord);
    }
}