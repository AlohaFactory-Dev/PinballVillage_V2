using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class BuildingManager
{
    private FactoryManager _factoryManager;
    private Dictionary<BuildingGroupType, List<Building>> _playerBuildings = new();
    private Dictionary<BuildingGroupType, List<Building>> _enemyBuildings = new();
    private Dictionary<BuildingGroupType, List<Building>> _neutralBuildings = new();
    private List<Building> _allBuildings = new();

    public List<Castle> PlayerCastles { get; private set; } = new();
    public List<EnemyCastle> EnemyCastles { get; private set; } = new();

    private readonly float _recoveryMultiple = 0f;

    public BuildingManager(FactoryManager factoryManager, StageGlobalClock stageGlobalClock)
    {
        _factoryManager = factoryManager;
        var etcTable = TableListContainer.Get<EtcTableList>().GetEtcTable("recoveryHp");
        _recoveryMultiple = etcTable.values[1];
        stageGlobalClock.RegisterRepeatingTimer("recoveryHp", etcTable.values[0], RecoverBuildHp);
    }

    public Building SpawnBuilding(string buildingId, Spawner spawner, bool isLevelUp = false)
    {
        var table = TableListContainer.Get<BuildingTableList>().GetBuildingTable(buildingId);
        var building = _factoryManager.BuildingFactory.GetBuilding(buildingId);
        if (building != null)
        {
            _allBuildings.Add(building);
            if (table.group == BuildingGroupType.DirectionSign)
            {
                (building as DirectionSign)?.Init(table, spawner, (Direction)Random.Range(0, 4), isLevelUp);
            }
            else
            {
                building.Init(table, spawner, isLevelUp);
            }

            if (spawner.CurrentOwner == OwnerType.Player)
            {
                if (table.group == BuildingGroupType.Castle)
                {
                    PlayerCastles.Add(building as Castle);
                }

                if (!_playerBuildings.ContainsKey(table.group))
                {
                    _playerBuildings[table.group] = new List<Building>();
                }

                _playerBuildings[table.group].Add(building);
            }
            else if (spawner.CurrentOwner == OwnerType.Enemy)
            {
                if (table.group == BuildingGroupType.Castle)
                {
                    EnemyCastles.Add(building as EnemyCastle);
                }

                if (!_enemyBuildings.ContainsKey(table.group))
                {
                    _enemyBuildings[table.group] = new List<Building>();
                }

                _enemyBuildings[table.group].Add(building);
            }
            else
            {
                if (!_neutralBuildings.ContainsKey(table.group))
                {
                    _neutralBuildings[table.group] = new List<Building>();
                }

                _neutralBuildings[table.group].Add(building);
            }
        }
        else
        {
            Debug.LogError($"Building with ID '{buildingId}' could not be found.");
        }


        return building;
    }

    public void RemoveBuilding(Building building)
    {
        // 빌딩 제거 로직
        if (building.OwnerType == OwnerType.Player && _playerBuildings.ContainsKey(building.Table.group))
        {
            _playerBuildings[building.Table.group].Remove(building);
        }
        else if (building.OwnerType == OwnerType.Enemy && _enemyBuildings.ContainsKey(building.Table.group))
        {
            _enemyBuildings[building.Table.group].Remove(building);
        }
        else if (building.OwnerType == OwnerType.Neutral && _neutralBuildings.ContainsKey(building.Table.group))
        {
            _neutralBuildings[building.Table.group].Remove(building);
        }
        else
        {
            Debug.LogWarning($"Building with ID '{building.Table.id}' not found in manager.");
        }

        _allBuildings.Remove(building);
        building.RemoveBuilding();
    }

    public void RestoreCastle(Building building)
    {
        if (building.OwnerType == OwnerType.Player)
        {
            if (!PlayerCastles.Contains(building as Castle))
            {
                PlayerCastles.Add(building as Castle);
            }

            if (!_playerBuildings.ContainsKey(BuildingGroupType.Castle))
            {
                _playerBuildings[BuildingGroupType.Castle] = new List<Building>();
            }

            _playerBuildings[BuildingGroupType.Castle].Add(building);
        }
        else if (building.OwnerType == OwnerType.Enemy)
        {
            if (!EnemyCastles.Contains(building as EnemyCastle))
            {
                EnemyCastles.Add(building as EnemyCastle);
            }

            if (!_enemyBuildings.ContainsKey(BuildingGroupType.Castle))
            {
                _enemyBuildings[BuildingGroupType.Castle] = new List<Building>();
            }

            _enemyBuildings[BuildingGroupType.Castle].Add(building);
        }
        else
        {
            Debug.LogWarning($"Building with ID '{building.Table.id}' is not a castle.");
            return;
        }

        _allBuildings.Add(building);
    }

    public void RemoveCastle(Building building)
    {
        if (building.OwnerType == OwnerType.Player)
        {
            PlayerCastles.Remove(building as Castle);
            _playerBuildings[BuildingGroupType.Castle].Remove(building);
            if (PlayerCastles.Count == 0)
            {
                StageContainer.Get<StageManager>().StageResult(OwnerType.Enemy);
            }
        }
        else if (building.OwnerType == OwnerType.Enemy)
        {
            EnemyCastles.Remove(building as EnemyCastle);
            _enemyBuildings[BuildingGroupType.Castle].Remove(building);
            if (EnemyCastles.Count == 0)
            {
                StageContainer.Get<StageManager>().StageResult(OwnerType.Player);
            }
        }
        else
        {
            Debug.LogWarning($"Building with ID '{building.Table.id}' is not a castle.");
            return;
        }

        _allBuildings.Remove(building);
    }

    private void RecoverBuildHp()
    {
        for (int i = 0; i < _allBuildings.Count; i++)
        {
            _allBuildings[i].RecoverHp(_recoveryMultiple);
        }
    }

    public Spawner GetCastle(OwnerType ownerType)
    {
        if (ownerType == OwnerType.Player)
        {
            int selectedIndex = Random.Range(0, PlayerCastles.Count);
            Castle playerCastle = PlayerCastles[selectedIndex];
            return playerCastle.Spawner;
        }

        if (ownerType == OwnerType.Enemy)
        {
            int selectedIndex = Random.Range(0, EnemyCastles.Count);
            EnemyCastle enemyCastle = EnemyCastles[selectedIndex];
            return enemyCastle.Spawner;
        }

        return null;
    }

    public Building LevelUpBuilding(Spawner spawner)
    {
        var buildingTable = spawner.Building.Table;
        RemoveBuilding(spawner.Building);
        var nextBuildingTable = TableListContainer.Get<BuildingTableList>().GetBuildingTableByGroup(buildingTable.levelUpTargetGroup, buildingTable.level + 1);
        return SpawnBuilding(nextBuildingTable.id, spawner, true);
    }

#if UNITY_EDITOR
    public void PlayerAllBuildingLevelUp()
    {
        foreach (var building in _playerBuildings)
        {
            // 복사본 생성
            var buildingsCopy = new List<Building>(building.Value);
            foreach (var b in buildingsCopy)
            {
                if (b.Table.level < b.Table.maxLevel)
                {
                    LevelUpBuilding(b.Spawner);
                }
            }
        }
    }

    public void SelectedBuildingLevelUp(string id)
    {
        var buildings = _playerBuildings.SelectMany(b => b.Value)
            .Where(b => b.Table.id == id && b.Table.level < b.Table.maxLevel);
        foreach (var building in buildings)
        {
            LevelUpBuilding(building.Spawner);
        }
    }
#endif
}