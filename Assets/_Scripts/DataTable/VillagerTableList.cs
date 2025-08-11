using System;
using System.Collections.Generic;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum VillagerType
{
    Normal,
    Lord,
    Archer,
    Building,
}

[Serializable]
public class VillagerTable
{
    [CSVColumn] public string id;
    [CSVColumn] public OwnerType ownerType;
    [CSVColumn] public int attackPower;
    [CSVColumn] public string attackObjectId;
    [CSVColumn] public VillagerType villagerType;
    [CSVColumn] public float moveSpeed;
    [CSVColumn] public List<float> values;
}

public class VillagerTableList : ITableList
{
    private List<VillagerTable> _tables = new List<VillagerTable>();
    private readonly Dictionary<(VillagerType, OwnerType), VillagerTable> _cachedTables = new();

    public async UniTask Init()
    {
        _tables = await TableManager.GetAsync<VillagerTable>("Villager");
        _cachedTables.Clear();
    }


    public VillagerTable GetVillagerTable(VillagerType villagerType, OwnerType ownerType)
    {
        var key = (villagerType, ownerType);
        if (_cachedTables.TryGetValue(key, out var objectInfo))
        {
            return objectInfo;
        }

        var info = _tables.Find(a => a.villagerType == villagerType && a.ownerType == ownerType);
        if (info == null)
        {
            Debug.LogError($"VillagerTableList not found. villagerType: {villagerType}, ownerType: {ownerType}");
            return null;
        }

        _cachedTables.Add(key, info);
        return info;
    }
}