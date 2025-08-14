using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3,
}

public class DirectionSign : Building
{
    [SerializeField] private new Transform renderer;
    private float _cooldownSeconds; // Villager별 쿨타임(초)
    private Direction _currentDirection;

    // Villager별 마지막 Push 시간 저장용 Dictionary
    private Dictionary<Villager, float> villagerCooldowns = new();
    private List<Villager> toRemove = new();

    public void Init(BuildingTable table, Spawner spawner, Direction initialDirection, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        _cooldownSeconds = Table.values[1];
        Collider2D.isTrigger = true;
        ChangeDirection(initialDirection);
        villagerCooldowns.Clear();
    }


    public void ChangeDirection(Direction direction)
    {
        _currentDirection = direction;
        switch (direction)
        {
            case Direction.Left:
                renderer.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.Right:
                renderer.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Up:
                renderer.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Down:
                renderer.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    public void ChangeDirectionSelf()
    {
        var nextDirection = (int)_currentDirection + 1;
        if (nextDirection > 3)
        {
            nextDirection = 0;
        }

        ChangeDirection((Direction)nextDirection);
    }

    private void Update()
    {
        float now = Time.time;
        toRemove.Clear();
        foreach (var pair in villagerCooldowns)
        {
            if (now - pair.Value >= _cooldownSeconds)
            {
                toRemove.Add(pair.Key);
            }
        }

        foreach (var villager in toRemove)
        {
            villagerCooldowns.Remove(villager);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Table.triggerTiming == TriggerTiming.OnCollision && other.gameObject.TryGetComponent(out Villager villager) && villager.OwnerType == OwnerType)
        {
            if (villagerCooldowns.ContainsKey(villager))
            {
                // 쿨타임 중이면 Push 무시
                return;
            }

            // Push 처리 및 쿨타임 등록
            villagerCooldowns[villager] = Time.time;

            var pushVillager = BuildingFunction as PushVillager;
            pushVillager.SetVillager(villager, _currentDirection);
        }
    }

    protected override void PerformAction(IChanger changer)
    {
    }
}