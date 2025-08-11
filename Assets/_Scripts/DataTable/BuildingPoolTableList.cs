using System;
using System.Collections.Generic;
using System.Linq;
using _DataTable.Script;
using Aloha.Coconut;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BuildingPoolTable
{
    [CSVColumn] public string id;
    [CSVColumn] public int maxAmount;
    [CSVColumn] public List<int> weight;
    [CSVColumn] public List<int> weightStepThresholds;
}

public class BuildingPoolTableList : ITableList
{
    private List<BuildingPoolTable> _etcTableList = new List<BuildingPoolTable>();

    public async UniTask Init()
    {
        _etcTableList = await TableManager.GetAsync<BuildingPoolTable>("BuildingPool");
    }

    public List<BuildingPoolTable> GetTables()
    {
        return _etcTableList.ToList();
    }
}