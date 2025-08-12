using System.Collections.Generic;
using UnityEngine;

// <summary>
// PassiveContainer는 Spawner에 속한 모든 Passive를 관리합니다.
// Passive는 건물이 생성될 때 Spawner의 현재 소유자에 따라 활성화됩니다.
// </summary>
public class SpawnerPassiveContainer
{
    private readonly Dictionary<string, List<Passive>> _passiveDic = new();

    private readonly Spawner _spawner;

    public SpawnerPassiveContainer(Spawner spawner)
    {
        _spawner = spawner;
    }


    public void AddPassive(Passive passive)
    {
        if (!_passiveDic.ContainsKey(passive.PassiveId))
        {
            _passiveDic[passive.PassiveId] = new List<Passive>();
        }

        if (!_spawner.IsEmpty)
        {
            _spawner.Building.UpdatePerformance(passive);
        }

        _passiveDic[passive.PassiveId].Add(passive);
    }

    public void RemovePassive(Passive passive)
    {
        if (_passiveDic.ContainsKey(passive.PassiveId))
        {
            _passiveDic[passive.PassiveId].Remove(passive);
            if (_passiveDic[passive.PassiveId].Count == 0)
            {
                _passiveDic.Remove(passive.PassiveId);
            }

            if (!_spawner.IsEmpty)
            {
                _spawner.Building.DowngradePerformance(passive);
            }
        }
        else
        {
            Debug.LogWarning($"Passive with ID {passive.PassiveId} not found in SpawnerPassiveContainer.");
        }
    }

    public void ActivePassives()
    {
        foreach (var passives in _passiveDic)
        {
            foreach (var passive in passives.Value)
            {
                if (passive.OwnerType == _spawner.CurrentOwner)
                {
                    _spawner.Building.UpdatePerformance(passive);
                }
            }
        }
    }
}