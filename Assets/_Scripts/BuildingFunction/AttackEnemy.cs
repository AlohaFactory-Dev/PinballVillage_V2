using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using _Scripts.Unit;
using FactorySystem;
using Stage.Building;

public class AttackEnemy : BuildingFunction, IChanger
{
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [Inject] private FactoryManager _factoryManager;
    public Transform Transform => transform;
    public VillagerType VillagerType => VillagerType.Building;
    public OwnerType OwnerType => Spawner.CurrentOwner;
    public int AttackPower => Table.attackPower;
    public VillagerMoveSystem VillagerMoveSystem { get; }

    public override void PerformAction(ActionContext actionContext)
    {
        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);

        var enemySpawners = neighbors.Where(neighbor =>
        {
            if (neighbor.CurrentOwner != Spawner.CurrentOwner)
            {
                if (!neighbor.IsEmpty)
                {
                    return neighbor.Building.Table.group != BuildingGroupType.Rock;
                }

                return true;
            }

            return false;
        }).ToList();

        if (enemySpawners.Count == 0) return;

        int attackCount = Mathf.Min((int)Table.effectValue + UpgradeValue, enemySpawners.Count);

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

    public override void UpgradePerformance(Passive passive)
    {
        UpgradeValue += passive.UpgradeValue;
    }

    public override void DowngradePerformance(Passive passive)
    {
        UpgradeValue -= passive.UpgradeValue;
        if (UpgradeValue < 0)
        {
            UpgradeValue = 0; // 공격 횟수는 음수가 될 수 없음
        }
    }

    public void SpawnerChangeAction(Spawner spawner)
    {
        // Do nothing
    }

    public override void DestroyAction()
    {
        // Do nothing
    }
}