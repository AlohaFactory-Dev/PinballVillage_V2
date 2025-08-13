using System;
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
    private Direction _currentDirection;

    public void Init(BuildingTable table, Spawner spawner, Direction initialDirection, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        Collider2D.isTrigger = true;
        ChangeDirection(initialDirection);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Table.triggerTiming == TriggerTiming.OnCollision && other.gameObject.TryGetComponent(out Villager villager) && villager.OwnerType == OwnerType)
        {
            var psuhVillager = BuildingFunction as PushVillager;
            psuhVillager.SetVillager(villager, _currentDirection);
        }
    }

    protected override void PerformAction(IChanger changer)
    {
    }
}