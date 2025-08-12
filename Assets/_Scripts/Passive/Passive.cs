using System.Collections.Generic;
using Stage.Building;
using UnityEngine;

public abstract class Passive
{
    public OwnerType OwnerType => _castSpawner.CurrentOwner;

    // 패시브를 발동 시킨 스포너
    private Spawner _castSpawner;

    // 패시브 받은 스포너들
    protected List<Spawner> InjectedPassiveSpawners = new();
    protected BuildingPassiveTable PassiveTable;
    protected SpawnerGridManager SpawnerGridManager;
    public string PassiveId => PassiveTable.id;
    public int UpgradeValue => (int)PassiveTable.effectValue;
    public BuildingGroupType TargetGroupType => PassiveTable.targetGroup;

    public void Init(Spawner spawner, string passiveId)
    {
        SpawnerGridManager = StageContainer.Get<SpawnerGridManager>();
        PassiveTable = TableListContainer.Get<BuildingPassiveTableList>().GetPassiveTable(passiveId);
        _castSpawner = spawner;
        InjectPassive(spawner);
    }

    protected abstract void InjectPassive(Spawner spawner);

    public void RemovePassive()
    {
        foreach (var spawner in InjectedPassiveSpawners)
        {
            spawner.RemovePassive(this);
        }

        InjectedPassiveSpawners.Clear();
        _castSpawner = null;
    }
}