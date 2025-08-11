using System;
using System.Collections.Generic;
using System.Linq;
using Stage.Building;
using UniRx;

public class EnemyAutoBuildManager
{
    private StageGlobalClock _stageGlobalClock;
    private SpawnerGridManager _spawnerGridManager;
    private BuildingManager _buildingManager;
    private List<Spawner> _autoBuildSpawners = new List<Spawner>();

    public EnemyAutoBuildManager(StageGlobalClock stageGlobalClock, SpawnerGridManager spawnerGridManager, BuildingManager buildingManager)
    {
        _stageGlobalClock = stageGlobalClock;
        _spawnerGridManager = spawnerGridManager;
        _buildingManager = buildingManager;

        _stageGlobalClock.CurrentTick.Subscribe(AutoBuildEnemy).AddTo(spawnerGridManager);
    }

    private void AutoBuildEnemy(int tick)
    {
        if (_autoBuildSpawners.Count == 0)
            return;

        var toBuild = new List<Spawner>();
        int playerPercent = _spawnerGridManager.GetPlayerSpawnerPercentage();

        foreach (var spawner in _autoBuildSpawners)
        {
            bool percentCondition = spawner.spawnPercent > 0 && playerPercent >= spawner.spawnPercent;
            bool timeCondition = spawner._autoSpawnTime > 0 && spawner._autoSpawnTime == tick;
            if ((percentCondition || timeCondition) && spawner.IsEmpty && spawner.CurrentOwner == OwnerType.Enemy)
            {
                toBuild.Add(spawner);
            }
        }

        foreach (var spawner in toBuild)
        {
            var building = _buildingManager.SpawnBuilding(spawner.autoBuildingId, spawner);
            if (building is DirectionSign dirSign)
            {
                dirSign.ChangeDirection(spawner.defaultDirection);
            }

            _autoBuildSpawners.Remove(spawner);
        }
    }

    public void ResistEnemyAutoBuild(Spawner spawner)
    {
        if (!_autoBuildSpawners.Contains(spawner))
            _autoBuildSpawners.Add(spawner);
    }
}