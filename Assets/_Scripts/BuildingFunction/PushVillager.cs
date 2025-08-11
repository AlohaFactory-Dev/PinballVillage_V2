using System;
using UnityEngine;

public class PushVillager : BuildingFunction
{
    private Direction _direction;

    public void SetVillager(Villager villager, Direction direction)
    {
        _direction = direction;
        PerformAction(villager, 0, Building.CalculateType.Add);
    }

    public override void PerformAction(IChanger changer, int value, Building.CalculateType calculate)
    {
        changer.VillagerMoveSystem?.BoostSpeedToDirection(Table.effectValue, Table.values[0], _direction);
    }


    public override void UpgradePerformance(float value)
    {
    }

    public override void DestroyAction()
    {
        // Do nothing for now
    }
}