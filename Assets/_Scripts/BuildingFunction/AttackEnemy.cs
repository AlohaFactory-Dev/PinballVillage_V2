using Stage.Building;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Unit;
using FactorySystem;

public class AttackEnemy : BuildingFunction, IChanger
{
    public Transform Transform => transform;
    public VillagerType VillagerType => VillagerType.Building;
    public OwnerType OwnerType => Spawner.CurrentOwner;
    public int AttackPower => (int)Table.attackPower;
    public VillagerMoveSystem VillagerMoveSystem { get; }
    [Inject] SpawnerGridManager _spawnerGridManager;
    [Inject] FactoryManager _factoryManager;
    private int _upgradeAttackCount = 0;

    public override void PerformAction(ActionContext actionContext)
    {
        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);

        var enemySpawners = neighbors.Where(neighbor =>
        {
            if (neighbor.CurrentOwner != Spawner.CurrentOwner)
            {
                if (!neighbor.IsEmpty)
                {
                    return neighbor.Building.Table.passiveTargetGroup != BuildingGroupType.Rock;
                }

                return true;
            }

            return neighbor.CurrentOwner != Spawner.CurrentOwner;
        }).ToList();

        if (enemySpawners.Count == 0) return;

        int attackCount = Mathf.Min((int)Table.effectValue + _upgradeAttackCount, enemySpawners.Count);

        for (int i = 0; i < attackCount; i++)
        {
            var randomTarget = enemySpawners[Random.Range(0, enemySpawners.Count)];
            FireArrow(randomTarget);
            enemySpawners.Remove(randomTarget); // 중복 선택 방지
        }
    }

    private void FireArrow(Spawner target)
    {
        var attackObjectTable = TableListContainer.Get<AttackObjectTableList>().GetAttackObjectTable(Table.attackObjectId);
        var attackObject = _factoryManager.AttackObjectFactory.GetAttackObject(Table.attackObjectId);
        attackObject.Init(transform.position, () => { target.ChangeOwner(this); }, attackObjectTable, target.transform.position);
    }

    public override void UpgradePerformance(float value)
    {
        _upgradeAttackCount += (int)value;
    }

    public void SpawnerChangeAction(Spawner spawner)
    {
    }

    public override void DestroyAction()
    {
        // Do nothing for now
    }
}