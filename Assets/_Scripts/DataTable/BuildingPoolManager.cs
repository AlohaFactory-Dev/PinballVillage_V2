using System.Collections.Generic;
using UnityEngine;

public class BuildingPoolManager
{
    private class PoolEntry
    {
        public BuildingTable Table;
        public BuildingPoolTable PoolTable;
        public int IndividualDrawCount; // 개별 빌딩 뽑기 횟수 (maxAmount 체크용)
    }

    private List<PoolEntry> _poolEntries = new List<PoolEntry>();
    private int _globalDrawCount = 0; // 전체 DrawCount

    public BuildingPoolManager()
    {
        BuildingTableList buildingTableList = TableListContainer.Get<BuildingTableList>();
        BuildingPoolTableList poolTableList = TableListContainer.Get<BuildingPoolTableList>();
        GeneratePool(buildingTableList, poolTableList);
    }

    private void GeneratePool(BuildingTableList buildingTableList, BuildingPoolTableList poolTableList)
    {
        _poolEntries.Clear();
        _globalDrawCount = 0;
        var poolTables = poolTableList.GetTables();

        foreach (var poolTable in poolTables)
        {
            var buildingTable = buildingTableList.GetBuildingTable(poolTable.id);
            if (buildingTable != null)
            {
                _poolEntries.Add(new PoolEntry
                {
                    Table = buildingTable,
                    PoolTable = poolTable,
                    IndividualDrawCount = 0
                });
            }
        }
    }

    private int GetWeightIndex(BuildingPoolTable poolTable)
    {
        var thresholds = poolTable.weightStepThresholds;
        int idx = 0;
        if (thresholds != null && thresholds.Count > 0)
        {
            for (int i = 0; i < thresholds.Count; i++)
            {
                if (_globalDrawCount < thresholds[i])
                {
                    idx = i;
                    break;
                }

                idx = i + 1;
            }
        }

        return Mathf.Min(idx, poolTable.weight.Count - 1);
    }

    public BuildingTable Draw()
    {
        if (TestManager.OnSettingBuildingCardMode)
        {
            return TestManager.GetSettingBuildingTable();
        }

        var candidates = new List<PoolEntry>();
        var weights = new List<int>();
        int totalWeight = 0;

        // maxAmount 미만 후보 추출
        foreach (var entry in _poolEntries)
        {
            if (entry.IndividualDrawCount < entry.PoolTable.maxAmount)
            {
                int weightIdx = GetWeightIndex(entry.PoolTable);
                int weight = (entry.PoolTable.weight != null && entry.PoolTable.weight.Count > weightIdx)
                    ? entry.PoolTable.weight[weightIdx]
                    : 1;
                candidates.Add(entry);
                weights.Add(weight);
                totalWeight += weight;
            }
        }

        // 후보가 없으면 모든 PoolEntry 대상으로 마지막 weight 인덱스 사용
        if (candidates.Count == 0)
        {
            foreach (var entry in _poolEntries)
            {
                int lastIdx = entry.PoolTable.weight.Count - 1;
                int weight = entry.PoolTable.weight[lastIdx];
                candidates.Add(entry);
                weights.Add(weight);
                totalWeight += weight;
            }
        }

        if (candidates.Count == 0) return null;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
            {
                // 개별 DrawCount 증가
                if (candidates[i].IndividualDrawCount < candidates[i].PoolTable.maxAmount)
                    candidates[i].IndividualDrawCount++;
                // 전체 DrawCount 증가
                _globalDrawCount++;
                return candidates[i].Table;
            }
        }

        // fallback
        if (candidates[^1].IndividualDrawCount < candidates[^1].PoolTable.maxAmount)
            candidates[^1].IndividualDrawCount++;
        _globalDrawCount++;
        return candidates[^1].Table;
    }

    public BuildingTable RefreshDraw(BuildingTable table)
    {
        if (TestManager.OnSettingBuildingCardMode)
        {
            return TestManager.GetSettingBuildingTable();
        }

        var entry1 = _poolEntries.Find(e => e.Table.id == table.id);
        if (entry1.IndividualDrawCount > 0)
            entry1.IndividualDrawCount--; // 개별 DrawCount 초기화
        var candidates = new List<PoolEntry>();
        var weights = new List<int>();
        int totalWeight = 0;

        foreach (var entry in _poolEntries)
        {
            if (entry.IndividualDrawCount < entry.PoolTable.maxAmount)
            {
                int weightIdx = GetWeightIndex(entry.PoolTable);
                int weight = (entry.PoolTable.weight != null && entry.PoolTable.weight.Count > weightIdx)
                    ? entry.PoolTable.weight[weightIdx]
                    : 1;
                candidates.Add(entry);
                weights.Add(weight);
                totalWeight += weight;
            }
        }

        if (candidates.Count == 0)
        {
            foreach (var entry in _poolEntries)
            {
                int lastIdx = entry.PoolTable.weight.Count - 1;
                int weight = entry.PoolTable.weight[lastIdx];
                candidates.Add(entry);
                weights.Add(weight);
                totalWeight += weight;
            }
        }

        if (candidates.Count == 0) return null;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
            {
                // GlobalCount를 증가시키지 않음
                if (candidates[i].IndividualDrawCount < candidates[i].PoolTable.maxAmount)
                    candidates[i].IndividualDrawCount++;
                return candidates[i].Table;
            }
        }

        if (candidates[^1].IndividualDrawCount < candidates[^1].PoolTable.maxAmount)
            candidates[^1].IndividualDrawCount++;
        return candidates[^1].Table;
    }
}