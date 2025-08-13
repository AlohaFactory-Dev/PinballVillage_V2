using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Building
{
    private int TargetCount => (int)Table.effectValue;

    protected override void PerformAction(IChanger changer)
    {
        BuildingFunction.PerformAction(new ActionContext(TargetCount));
    }
}