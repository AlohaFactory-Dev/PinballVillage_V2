using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : MonoBehaviour
{
    public OwnerType OwnerType => CastSpawner.CurrentOwner;

    // 패시브를 발동 시킨 스포너
    protected Spawner CastSpawner { get; set; }

    // 패시브 받은 스포너들
    protected List<Spawner> InjectedPassiveSpawners = new();

    public void AddPassive(Spawner spawner)
    {
        CastSpawner = spawner;
        InjectPassive(spawner);
    }

    protected abstract void InjectPassive(Spawner spawner);

    public void RemovePassive()
    {
        foreach (var spawner in InjectedPassiveSpawners)
        {
            spawner.RemovePassive(this);
        }

        InjectedPassiveSpawners.Clear();
        CastSpawner = null;
    }

    public abstract void Activate(Spawner spawner);
}