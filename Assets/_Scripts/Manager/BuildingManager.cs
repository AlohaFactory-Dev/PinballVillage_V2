using System.Collections;
using System.Collections.Generic;
using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class BuildingManager
{
    private FactoryManager _factoryManager;
    private StageGlobalClock _stageGlobalClock;
    private Dictionary<BuildingGroupType, List<Building>> _playerBuildings = new();
    private Dictionary<BuildingGroupType, List<Building>> _enemyBuildings = new();
    private Dictionary<BuildingGroupType, List<Building>> _neutralBuildings = new();
    private List<Building> _allBuildings = new();

    public List<Castle> PlayerCastles { get; private set; } = new();
    public List<EnemyCastle> EnemyCastles { get; private set; } = new();

    private float _recoveryMultiple = 0f;

    public BuildingManager(FactoryManager factoryManager, StageGlobalClock stageGlobalClock)
    {
        _factoryManager = factoryManager;
        _stageGlobalClock = stageGlobalClock;
        var etcTable = TableListContainer.Get<EtcTableList>().GetEtcTable("recoveryHp");
        _recoveryMultiple = etcTable.values[1];
        _stageGlobalClock.RegisterRepeatingTimer("recoveryHp", etcTable.values[0], RecoverBuildHp);
    }

    public Building SpawnBuilding(string buildingId, Spawner spawner, bool isLevelUp = false)
    {
        var table = TableListContainer.Get<BuildingTableList>().GetBuildingTable(buildingId);
        var building = _factoryManager.BuildingFactory.GetBuilding(buildingId);
        if (building != null)
        {
            _allBuildings.Add(building);
            if (table.passiveTargetGroup == BuildingGroupType.DirectionSign)
            {
                (building as DirectionSign)?.Init(table, spawner, (Direction)Random.Range(0, 4), isLevelUp);
            }
            else
            {
                building.Init(table, spawner, isLevelUp);
            }

            if (spawner.CurrentOwner == OwnerType.Player)
            {
                if (table.passiveTargetGroup == BuildingGroupType.Castle)
                {
                    PlayerCastles.Add(building as Castle);
                }

                if (!_playerBuildings.ContainsKey(table.passiveTargetGroup))
                {
                    _playerBuildings[table.passiveTargetGroup] = new List<Building>();
                }

                _playerBuildings[table.passiveTargetGroup].Add(building);
            }
            else if (spawner.CurrentOwner == OwnerType.Enemy)
            {
                if (table.passiveTargetGroup == BuildingGroupType.Castle)
                {
                    EnemyCastles.Add(building as EnemyCastle);
                }

                if (!_enemyBuildings.ContainsKey(table.passiveTargetGroup))
                {
                    _enemyBuildings[table.passiveTargetGroup] = new List<Building>();
                }

                _enemyBuildings[table.passiveTargetGroup].Add(building);
            }
            else
            {
                if (!_neutralBuildings.ContainsKey(table.passiveTargetGroup))
                {
                    _neutralBuildings[table.passiveTargetGroup] = new List<Building>();
                }

                _neutralBuildings[table.passiveTargetGroup].Add(building);
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
        if (building.OwnerType == OwnerType.Player && _playerBuildings.ContainsKey(building.Table.passiveTargetGroup))
        {
            _playerBuildings[building.Table.passiveTargetGroup].Remove(building);
            if (building.Table.passiveTargetGroup == BuildingGroupType.Castle)
            {
                PlayerCastles.Remove(building as Castle);
                if (PlayerCastles.Count == 0)
                {
                    StageContainer.Get<StageManager>().StageResult(OwnerType.Enemy);
                }
            }
        }
        else if (building.OwnerType == OwnerType.Enemy && _enemyBuildings.ContainsKey(building.Table.passiveTargetGroup))
        {
            _enemyBuildings[building.Table.passiveTargetGroup].Remove(building);

            if (building.Table.passiveTargetGroup == BuildingGroupType.Castle)
            {
                EnemyCastles.Remove(building as EnemyCastle);
                if (EnemyCastles.Count == 0)
                {
                    StageContainer.Get<StageManager>().StageResult(OwnerType.Player);
                }
            }
        }
        else if (building.OwnerType == OwnerType.Neutral && _neutralBuildings.ContainsKey(building.Table.passiveTargetGroup))
        {
            _neutralBuildings[building.Table.passiveTargetGroup].Remove(building);
        }
        else
        {
            Debug.LogWarning($"Building with ID '{building.Table.id}' not found in manager.");
        }

        _allBuildings.Remove(building);
        building.RemoveBuilding();
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
        spawner.Building.RemoveBuilding();
        var nextBuildingTable = TableListContainer.Get<BuildingTableList>().GetBuildingTableByGroup(buildingTable.levelUpTargetGroup, buildingTable.level + 1);
        return SpawnBuilding(nextBuildingTable.id, spawner, true);
    }
}