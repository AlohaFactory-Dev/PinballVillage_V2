using Stage.Building;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class BankPassive : Passive
{
    protected override void InjectPassive(Spawner spawner)
    {
        InjectedPassiveSpawners = SpawnerGridManager.GetAllSpawners();
        foreach (var neighbor in InjectedPassiveSpawners)
        {
            neighbor.AddSpawnerPassive(this);
        }
    }
}