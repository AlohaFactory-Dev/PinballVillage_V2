using System.Linq;
using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class ArcherCharacter : Villager
{
    public override VillagerType VillagerType => VillagerType.Archer;
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [Inject] private FactoryManager _factoryManager;
    private int _upgradeAttackCount = 0;
    private int AttackRange => (int)Table.values[0]; // 기본 공격 범위
    private int AttackCount => (int)Table.values[1] + _upgradeAttackCount; // 기본 공격 횟수

    public override void SpawnerChangeAction(Spawner spawner)
    {
        var neighbors = _spawnerGridManager.GetNeighbors(spawner, AttackRange);
        var enemySpawners = neighbors.Where(neighbor =>
        {
            if (neighbor.CurrentOwner != OwnerType)
            {
                if (!neighbor.IsEmpty)
                {
                    return neighbor.Building.Table.group != BuildingGroupType.Rock;
                }

                return true;
            }

            return neighbor.CurrentOwner != OwnerType;
        }).ToList();

        if (enemySpawners.Count == 0) return;

        int attackCount = Mathf.Min(AttackCount, enemySpawners.Count);

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

    public void AddAttackCount(int value)
    {
        _upgradeAttackCount = value;
    }

    public void DecreaseAttackCount(int value)
    {
        _upgradeAttackCount = Mathf.Max(0, _upgradeAttackCount - value);
    }
}