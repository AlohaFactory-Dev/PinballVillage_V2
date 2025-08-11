using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class BankPassive : Passive
{
    [Inject] SpawnerGridManager _spawnerGridManager;
    [SerializeField] private int addGoldAmount = 3;

    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = _spawnerGridManager.GetAllSpawners();
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
            spawner.Building.UpdatePerformance(addGoldAmount);
        }
    }
}