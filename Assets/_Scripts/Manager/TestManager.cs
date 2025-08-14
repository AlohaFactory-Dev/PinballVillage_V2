using System;
using System.Collections;
using System.Collections.Generic;
using Aloha.Coconut;
using FactorySystem;
using Sirenix.OdinInspector;
using Stage.Building;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

# if UNITY_EDITOR

[InfoBox("G : 10000골드 추가" +
    "\nL : 건물 레벨업" +
    "\nUpArrow : Time Scale + 1" +
    "\nDownArrow : Time Scale - 1" +
    "\nRightArrow, LeftArrow : Time Scale = 1" +
    "\nI : 적과의 충돌 무시 토글" +
    "\nB : 건물 생성 시작")]
public class TestManager : MonoBehaviour
{
    [Serializable]
    public struct SpawnBuildingList
    {
        public string buildingId;
        public Vector2Int gridPosition;
    }

    [Serializable]
    public struct SpawnList
    {
        public List<SpawnBuildingList> spawnBuildingList;
    }

    [Inject] private GoldManager _goldManager;

    [Space]
    [InfoBox("모든 건물 레벨업을 할지, 특정 건물만 레벨업할지 선택하세요.")]
    [SerializeField]
    private bool allBuildingLevelUp = false;

    [SerializeField] private List<string> levelUpBuildingIds = new();

    [Space]
    [InfoBox("적과의 충돌을 무시할지 설정합니다.")]
    [SerializeField]
    private bool onEnemyCollisionIgnore = false;

    private static bool _onEnemyCollsionIgnore = false;

    [Space]
    [InfoBox("생성 할 건물 리스트입니다." +
        "\n각 건물의 ID와 생성할 위치를 설정하세요." +
        "\n생성할 위치는 그리드 좌표로 설정합니다." +
        "\nSpawnInterval은 각 건물이 생성될 때 시간 간격입니다."
    )]
    [SerializeField]
    private int spawnBuildingIndex;

    [SerializeField] private SpawnList[] spawnList;

    [SerializeField] private float spawnInterval = 0.1f;

    [Space]
    [InfoBox("이 모드를 활성화 하면 설정된 건물 카드가 생성됩니다." +
        "\n건물 카드의 ID는 SettingBuildingCardIds에 설정된 값들 중에서 순차적으로 사용됩니다." +
        "\n모드가 활성화되면 SettingBuildingCardIds의 첫 번째 ID가 사용되고, 사용 후에는 제거됩니다." +
        "\nSettingBuildingCardIds가 비어 있으면 모드가 비활성화됩니다."
    )]
    [SerializeField]
    private bool onSettingBuildingCardMode = false;

    private static bool _onSettingBuildingCardMode = false;
    [SerializeField] private List<string> settingBuildingCardIds;
    public static List<string> SettingBuildingCardIds;
    public static bool OnEnemyCollsionIgnore => _onEnemyCollsionIgnore;
    public static bool OnSettingBuildingCardMode => _onSettingBuildingCardMode;

    private void Start()
    {
        _onSettingBuildingCardMode = onSettingBuildingCardMode;
        _onEnemyCollsionIgnore = onEnemyCollisionIgnore;
        SettingBuildingCardIds = settingBuildingCardIds;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _goldManager.AddGold(10000);
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (allBuildingLevelUp)
            {
                StageContainer.Get<BuildingManager>().PlayerAllBuildingLevelUp();
            }
            else
            {
                foreach (var id in levelUpBuildingIds)
                {
                    StageContainer.Get<BuildingManager>().SelectedBuildingLevelUp(id);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            onEnemyCollisionIgnore = !onEnemyCollisionIgnore;
            _onEnemyCollsionIgnore = onEnemyCollisionIgnore;
            Debug.Log($"On Enemy Collision Ignore: {onEnemyCollisionIgnore}");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SpawnBuilding());
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale -= 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Time.timeScale = 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }
    }

    #region Spawn Building Mode

    private IEnumerator SpawnBuilding()
    {
        var buildingManager = StageContainer.Get<BuildingManager>();
        var spawnerGridManager = StageContainer.Get<SpawnerGridManager>();
        if (spawnBuildingIndex >= spawnList.Length)
        {
            Debug.Log("모든 건물 생성 완료");
            yield break;
        }

        var spawnData = spawnList[spawnBuildingIndex];
        foreach (var spawnBuilding in spawnData.spawnBuildingList)
        {
            var spawner = spawnerGridManager.GetSpawner(spawnBuilding.gridPosition);
            if (!spawner.IsEmpty)
            {
                buildingManager.RemoveBuilding(spawner.Building);
            }

            spawner.ResetOwner(OwnerType.Player);
            buildingManager.SpawnBuilding(spawnBuilding.buildingId, spawner);
            yield return new WaitForSeconds(spawnInterval);
        }

        spawnBuildingIndex++;
    }

    #endregion

    #region Setting Building Card Mode

    public static BuildingTable GetSettingBuildingTable()
    {
        string buildingId = SettingBuildingCardIds[0];
        SettingBuildingCardIds.RemoveAt(0);
        if (SettingBuildingCardIds.Count == 0)
        {
            _onSettingBuildingCardMode = false;
        }

        return TableListContainer.Get<BuildingTableList>().GetBuildingTable(buildingId);
    }

    #endregion
}
#endif