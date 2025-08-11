using System;
using System.Collections.Generic;
using FactorySystem;
using ModestTree;
using UnityEngine;
using Zenject;
using Stage.Building;


public enum OwnerType
{
    Player,
    Enemy,
    Neutral
}

public class Spawner : MonoBehaviour
{
    private SpawnerPassiveContainer _spawnerPassiveContainer;
    private Building _building;

    [Inject] private SpawnerGridManager _spawnerGridManager;
    [Inject] private VillagerManager _villagerManager;
    [SerializeField] private OwnerType defaultOwnerType;
    [SerializeField] private int row = -1;
    [SerializeField] private int col = -1;
    [SerializeField] private SpawnerOwnerChangerController ownerChangerController;
    [SerializeField] private string defaultBuildingId;
    [SerializeField] private GameObject buildAbleHighlight;
    [SerializeField] private GameObject buildDisableHighlight;
    [SerializeField] private Animator showRangeHighlight;


    [Header("적 자동 건물 생성 관련")] public int _autoSpawnTime = 0;
    [Range(0, 100)] public int spawnPercent = 0;
    public string autoBuildingId = string.Empty;
    public Direction defaultDirection = Direction.Right;

    public Building Building => _building;
    public bool IsEmpty => _building == null;

    public OwnerType CurrentOwner => ownerChangerController.CurrentOwner;

    // 그리드 좌표 프로퍼티 (읽기 전용)
    public Vector2Int GridPosition => new Vector2Int(col, row);
    public void ChangeOwner(IChanger changer) => ownerChangerController.Change(changer);


    public void Init(int colIndex, int rowIndex)
    {
        col = colIndex;
        row = rowIndex;
        ownerChangerController.Init(defaultOwnerType, this);
        _spawnerPassiveContainer = new SpawnerPassiveContainer(this);
        AllBuildAbleHighlightOff();
        ShowRangeHighlight(false);
        if (!autoBuildingId.IsEmpty())
            StageContainer.Get<EnemyAutoBuildManager>().ResistEnemyAutoBuild(this);

        _spawnerGridManager.AddPercentage(this, CurrentOwner);
    }

    public void SpawnDefaultBuilding()
    {
        if (!defaultBuildingId.IsEmpty())
        {
            var building = StageContainer.Get<BuildingManager>().SpawnBuilding(defaultBuildingId, this);
            if (building as DirectionSign)
            {
                (building as DirectionSign).ChangeDirection(defaultDirection);
            }
        }
    }

    public void SetBuilding(Building building)
    {
        _building = building;
        _spawnerPassiveContainer.ActivePassives();
    }

    public void ClearBuilding()
    {
        _building = null;
    }

    public void AddSpawnerPassive(Passive passive)
    {
        _spawnerPassiveContainer.AddPassive(passive);
    }

    public void RemovePassive(Passive passive)
    {
        _spawnerPassiveContainer.RemovePassive(passive);
    }

    public void BuildAbleHighlight(bool isOn)
    {
        if (isOn)
        {
            buildAbleHighlight.SetActive(true);
            buildDisableHighlight.SetActive(false);
        }
        else
        {
            buildAbleHighlight.SetActive(false);
            buildDisableHighlight.SetActive(true);
        }
    }

    public void AllBuildAbleHighlightOff()
    {
        buildAbleHighlight.SetActive(false);
        buildDisableHighlight.SetActive(false);
    }

    public void ShowRangeHighlight(bool isOn)
    {
        if (isOn && !showRangeHighlight.gameObject.activeSelf)
        {
            // 애니메이션 시작
            showRangeHighlight.gameObject.SetActive(true);
            showRangeHighlight.SetTrigger("Show");
        }
        else if (!isOn && showRangeHighlight.gameObject.activeSelf)
        {
            showRangeHighlight.gameObject.SetActive(false);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!defaultBuildingId.IsEmpty())
        {
            if (defaultBuildingId == "Rock" || defaultBuildingId == "Tree" || defaultBuildingId == "DirectionSign")
            {
                defaultOwnerType = OwnerType.Neutral;
            }
        }

        ownerChangerController.ResetOwner(defaultOwnerType);
    }

    public void SetName()
    {
        gameObject.name = $"Spawner ({row}, {col}) - {defaultOwnerType}";
    }

    public void SetSprite(Dictionary<OwnerType, Sprite> sprite)
    {
        ownerChangerController.SetSprite(sprite);
    }

    private void OnDrawGizmos()
    {
        if (!defaultBuildingId.IsEmpty())
        {
            Color color = Color.red;
            if ("Rock".Equals(defaultBuildingId))
            {
                color = Color.gray;
            }
            else if ("Tree".Equals(defaultBuildingId))
            {
                color = Color.green;
            }
            else if ("WheatField_0".Equals(defaultBuildingId) || "WheatField_1".Equals(defaultBuildingId))
            {
                color = Color.yellow;
            }

            UnityEditor.Handles.Label(
                transform.position,
                defaultBuildingId,
                new GUIStyle()
                {
                    normal = { textColor = color },
                    fontSize = 12,
                    alignment = TextAnchor.MiddleCenter
                }
            );
        }

        if (!autoBuildingId.IsEmpty())
        {
            UnityEditor.Handles.Label(
                transform.position,
                $"Auto:\n{autoBuildingId} ({_autoSpawnTime}s) , {spawnPercent}%",
                new GUIStyle()
                {
                    normal = { textColor = Color.blue },
                    fontSize = 12,
                    alignment = TextAnchor.MiddleCenter
                }
            );
        }
    }
#endif
}