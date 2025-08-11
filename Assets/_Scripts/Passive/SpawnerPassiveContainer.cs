using System.Collections.Generic;
using UnityEngine;

// <summary>
// PassiveContainer는 Spawner에 속한 모든 Passive를 관리합니다.
// Passive는 건물이 생성될 때 Spawner의 현재 소유자에 따라 활성화됩니다.
// </summary>
public class SpawnerPassiveContainer
{
    private readonly List<Passive> _passives = new List<Passive>();

    private readonly Spawner _spawner;

    public SpawnerPassiveContainer(Spawner spawner)
    {
        _spawner = spawner;
    }


    public void AddPassive(Passive passive)
    {
        _passives.Add(passive);
    }

    public void RemovePassive(Passive passive)
    {
        if (!_passives.Remove(passive))
        {
            Debug.LogWarning($"Passive {passive.name} not found in SpawnerPassiveContainer.");
        }
    }

    public void ActivePassives()
    {
        foreach (var passive in _passives)
        {
            if (passive.OwnerType == _spawner.CurrentOwner)
            {
                passive.Activate(_spawner);
            }
        }
    }
}