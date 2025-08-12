using System;
using System.Collections.Generic;
using System.Linq;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum TriggerTiming
{
    None, // 없음
    OnSpawn, // 등장시
    OnCollision, // 활성화시
    Auto // 자동 (시간 기반)
}

public enum SpawnPointType
{
    Soil, Rampart
}

public class BuildingTable
{
    [CSVColumn] public BuildingGroupType group;
    [CSVColumn] public string levelUpTargetGroup;
    [CSVColumn] public string id;
    [CSVColumn] public int level;
    [CSVColumn] public string attackObjectId;
    [CSVColumn] public SpawnPointType spawnPointType;
    [CSVColumn] public int levelUpCost;
    [CSVColumn] public int buildCost;
    [CSVColumn] public int showRange;
    [CSVColumn] public TriggerTiming triggerTiming;
    [CSVColumn] public int attackPower;
    [CSVColumn] public float interval;
    [CSVColumn] public int targetRange;
    [CSVColumn] public float effectValue;
    [CSVColumn] public int grade;
    [CSVColumn] public List<float> values;
    [CSVColumn] public string iconPath;
    [CSVColumn] public string descriptionKey;
    [CSVColumn] public int maxHp;
    [CSVColumn] public string nameKey;
    [CSVColumn] public string passiveId;

    public int maxLevel;
    // [CSVColumn] public string passiveDescriptionKey;
}

public class BuildingTableList : ITableList
{
    private List<BuildingTable> _tables = new List<BuildingTable>();
    private readonly Dictionary<string, BuildingTable> _cachedTables = new Dictionary<string, BuildingTable>();

    public async UniTask Init()
    {
        _tables = await TableManager.GetAsync<BuildingTable>("Building");
        _cachedTables.Clear();
        foreach (var table in _tables)
        {
            table.maxLevel = _tables
                .Where(a => a.levelUpTargetGroup == table.levelUpTargetGroup)
                .Max(a => a.level);
        }
    }

    public BuildingTable GetBuildingTable(string id)
    {
        if (_cachedTables.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _tables.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"BuildingTable not found. id: {id}");
            return null;
        }

        _cachedTables.Add(id, info);
        return info;
    }

    public BuildingTable GetBuildingTableByGroup(string levelUpTargetGroup, int level = 1)
    {
        return _tables.Find(a => a.levelUpTargetGroup == levelUpTargetGroup && a.level == level);
    }
}