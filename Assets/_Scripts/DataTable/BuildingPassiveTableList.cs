using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class BuildingPassiveTable
{
    [CSVColumn] public string id;
    [CSVColumn] public int range;
    [CSVColumn] public int targetCount;
    [CSVColumn] public int effectValue;
    [CSVColumn] public List<float> values;
}

public class BuildingPassiveTableList : ITableList
{
    private List<BuildingPassiveTable> _tables = new List<BuildingPassiveTable>();
    private readonly Dictionary<string, BuildingPassiveTable> _cachedTables = new();

    public async UniTask Init()
    {
        _tables = await TableManager.GetAsync<BuildingPassiveTable>("Villager");
        _cachedTables.Clear();
    }


    public BuildingPassiveTable GetPassiveTable(string id)
    {
        if (_cachedTables.TryGetValue(id, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _tables.Find(a => a.id == id);
        if (info == null)
        {
            Debug.LogError($"VillagerTableList not found. id: {id}");
            return null;
        }

        _cachedTables.Add(id, info);
        return info;
    }
}