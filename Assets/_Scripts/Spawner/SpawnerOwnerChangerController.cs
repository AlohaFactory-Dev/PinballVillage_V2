using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Stage.Building
{
    public class SpawnerOwnerChangerController : MonoBehaviour
    {
        [Inject] private SpawnerGridManager _spawnerGridManager;
        private OwnerType _currentOwner;
        public OwnerType CurrentOwner => _currentOwner;
        [SerializeField] private SpawnerOwnerChanger[] ownerChangers;
        private Spawner _spawner;
        private int PlayerVillagerLayer => LayerMask.GetMask("Player");
        private int EnemyVillagerLayer => LayerMask.GetMask("Enemy");
        private int AllVillagerLayer => PlayerVillagerLayer | EnemyVillagerLayer;
        private SpawnerOwnerChanger _currentChanger;

        public void Init(OwnerType currentOwner, Spawner spawner)
        {
            _spawner = spawner;
            foreach (var changer in ownerChangers)
            {
                changer.Init(OnCollisionChange, _spawner);
            }

            _currentOwner = currentOwner;
        }

        private void OnCollisionChange(Villager villager)
        {
            villager.SpawnerChangeAction(_spawner);
            Change(villager);
        }

        public void Change(IChanger changer)
        {
            if (_currentOwner == OwnerType.Neutral && !_spawner.IsEmpty)
            {
                if (_spawner.Building.Table.group != BuildingGroupType.DirectionSign)
                {
                    if (_spawner.Building.Table.group == BuildingGroupType.WheatField)
                    {
                        HandleOwnerChange(changer);
                    }
                    else
                    {
                        _spawner.Building.OnCollisionFunction(changer);
                    }
                }
                else
                {
                    HandleOwnerChange(changer);
                }

                return;
            }

            if (changer.OwnerType != _currentOwner && !_spawner.IsEmpty)
            {
                if (_spawner.Building.HasHp)
                {
                    _spawner.Building.TakeDamage(changer.AttackPower);
                    return;
                }
            }

            HandleOwnerChange(changer);
        }

        private void HandleOwnerChange(IChanger changer)
        {
            if (changer.OwnerType == OwnerType.Player)
            {
                if (_currentOwner == OwnerType.Enemy)
                {
                    CheckOwnerChangers(OwnerType.Neutral);
                }
                else if (_currentOwner == OwnerType.Neutral)
                {
                    CheckOwnerChangers(OwnerType.Player);
                }
            }
            else if (changer.OwnerType == OwnerType.Enemy)
            {
                if (_currentOwner == OwnerType.Player)
                {
                    CheckOwnerChangers(OwnerType.Neutral);
                }
                else if (_currentOwner == OwnerType.Neutral)
                {
                    CheckOwnerChangers(OwnerType.Enemy);
                }
            }
        }

        private void CheckOwnerChangers(OwnerType ownerType)
        {
            foreach (var changer in ownerChangers)
            {
                if (changer.EnableCheck(ownerType))
                {
                    _currentChanger = changer;
                    _currentOwner = ownerType;
                    _spawnerGridManager.AddPercentage(_spawner, _currentOwner);
                }
            }

            if (_currentOwner == OwnerType.Player)
            {
                ReleaseVillager(OwnerType.Enemy, _currentChanger);
            }
            else if (_currentOwner == OwnerType.Enemy)
            {
                ReleaseVillager(OwnerType.Player, _currentChanger);
            }
            else
            {
                ReleaseVillager(OwnerType.Neutral, _currentChanger);
            }
        }


        private void ReleaseVillager(OwnerType targetType, SpawnerOwnerChanger changer)
        {
            Collider2D[] villagers;
            if (targetType == OwnerType.Player)
            {
                villagers = Physics2D.OverlapBoxAll(changer.BoxCollider.bounds.center, changer.BoxCollider.bounds.size, 0, PlayerVillagerLayer);
            }
            else if (targetType == OwnerType.Enemy)
            {
                villagers = Physics2D.OverlapBoxAll(changer.BoxCollider.bounds.center, changer.BoxCollider.bounds.size, 0, EnemyVillagerLayer);
            }
            else
            {
                villagers = Physics2D.OverlapBoxAll(changer.BoxCollider.bounds.center, changer.BoxCollider.bounds.size, 0, AllVillagerLayer);
            }

            if (villagers.Length > 0)
            {
                foreach (var villagerCollider in villagers)
                {
                    if (villagerCollider.TryGetComponent(out Villager villager))
                    {
                        villager.VillagerMoveSystem.JumpNearTheCastle();
                    }
                }
            }
        }
#if UNITY_EDITOR
        public void ResetOwner(OwnerType ownerType)
        {
            foreach (var changer in ownerChangers)
            {
                if (changer.EnableCheck(ownerType))
                {
                    _currentChanger = changer;
                    _currentOwner = ownerType;
                }
            }

            if (_currentOwner == OwnerType.Player)
            {
                ReleaseVillager(OwnerType.Enemy, _currentChanger);
            }
            else if (_currentOwner == OwnerType.Enemy)
            {
                ReleaseVillager(OwnerType.Player, _currentChanger);
            }
            else
            {
                ReleaseVillager(OwnerType.Neutral, _currentChanger);
            }
        }

        public void SetSprite(Dictionary<OwnerType, Sprite> sprites)
        {
            foreach (var changer in ownerChangers)
            {
                if (!sprites.TryGetValue(changer.CurrentOwner, out var sprite))
                {
                    continue;
                }

                changer.SetSprite(sprite);
            }
        }
#endif
    }
}