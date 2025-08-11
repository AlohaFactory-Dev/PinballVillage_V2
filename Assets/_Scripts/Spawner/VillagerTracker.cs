using System.Collections.Generic;
using UnityEngine;
using System;

public class VillagerTracker : MonoBehaviour
{
    [SerializeField] private List<Villager> villagerInSpawner = new List<Villager>();


    public IReadOnlyList<Villager> VillagerInSpawner => villagerInSpawner;
    private Action<Villager> _villagerAdded;
    private Action<Villager> _villagerRemoved;
    private VillagerType _villagerType;
    private Spawner _spawner;

    public void Init(Spawner spawner, Action<Villager> villagerAddedCallback, Action<Villager> villagerRemovedCallback)
    {
        _spawner = spawner;
        _villagerAdded = villagerAddedCallback;
        _villagerRemoved = villagerRemovedCallback;
    }

    public void CreateCrossColliders(Vector2 position)
    {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1, 1);
        collider.offset = (Vector2)transform.position - position;
        collider.usedByComposite = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Villager villager))
        {
            if (!villagerInSpawner.Contains(villager) && _spawner.CurrentOwner == villager.OwnerType)
            {
                villagerInSpawner.Add(villager);
                _villagerAdded?.Invoke(villager);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Villager villager))
        {
            if (villagerInSpawner.Contains(villager) && _spawner.CurrentOwner == villager.OwnerType)
            {
                villagerInSpawner.Remove(villager);
                _villagerRemoved?.Invoke(villager);
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 스포너 영역 표시
        Gizmos.color = Color.yellow;
        var colliders = GetComponents<BoxCollider2D>();
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
            }
        }

        // 현재 스포너 내의 Villager들 표시
        Gizmos.color = Color.red;
        foreach (var villager in villagerInSpawner)
        {
            if (villager != null)
            {
                Gizmos.DrawLine(transform.position, villager.transform.position);
                Gizmos.DrawWireSphere(villager.transform.position, 0.2f);
            }
        }

        // 스포너 중심점 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
#endif
}