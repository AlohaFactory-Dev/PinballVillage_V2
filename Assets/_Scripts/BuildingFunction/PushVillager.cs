using System;
using UnityEngine;

public class PushVillager : BuildingFunction
{
    private Direction _direction;
    private float Duration => Table.values[0];

    public void SetVillager(Villager villager, Direction direction)
    {
        _direction = direction;
        PerformAction(new ActionContext(villager));
    }

    public override void PerformAction(ActionContext actionContext)
    {
        actionContext.Changer.VillagerMoveSystem?.BoostSpeedToDirection(Table.effectValue, Duration, _direction);
    }


    public override void UpgradePerformance(Passive passive)
    {
    }

    public override void DowngradePerformance(Passive passive)
    {
    }

    public override void DestroyAction()
    {
        // Do nothing for now
    }
}