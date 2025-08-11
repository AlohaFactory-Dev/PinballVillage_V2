using System;
using System.Collections;
using System.Collections.Generic;
using Stage.Building;
using UnityEngine;
using Zenject;

public class Windmill : Building
{
    [Inject] SpawnerGridManager _spawnerGridManager;

    protected override void PerformAction(IChanger changer = null, int value = 0, CalculateType calculate = CalculateType.Add)
    {
        var neighbors = _spawnerGridManager.GetNeighbors(Spawner, Table.targetRange);
        int wheatCount = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.IsEmpty) continue;
            if (neighbor.Building.PassiveTargetGroupType == BuildingGroupType.WheatField && neighbor.CurrentOwner == Spawner.CurrentOwner)
            {
                wheatCount++;
            }
        }

        // Whidmill은 wheatCount에 따라 성능을 조정함.
        base.PerformAction(changer, wheatCount, calculate);
    }
}