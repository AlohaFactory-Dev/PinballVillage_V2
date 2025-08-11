using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class BankPassive : Passive
{
    private int AddGoldAmount => PassiveTable.effectValue;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = SpawnerGridManager.GetAllSpawners();
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            if (neighbor == spawner) continue;
            neighbor.AddSpawnerPassive(this);
            Activate(neighbor);
        }
    }

    public override void Activate(Spawner spawner)
    {
        if (spawner.IsEmpty) return;
        if (spawner.Building.PassiveTargetGroupType == BuildingGroupType.Market)
        {
            spawner.Building.UpdatePerformance(AddGoldAmount);
        }
    }
}