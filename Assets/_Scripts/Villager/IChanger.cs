using _Scripts.Unit;
using UnityEngine;

public interface IChanger
{
    public Transform Transform { get; }
    public VillagerType VillagerType { get; }
    public OwnerType OwnerType { get; }
    public int AttackPower { get; }
    public VillagerMoveSystem VillagerMoveSystem { get; }
    public void SpawnerChangeAction(Spawner spawner);
}