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
    "\nI : 적과의 충돌 무시 토글")]
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
    private static bool _onEnemyCollsionIgnore = false;

    [Space]
    [InfoBox("생성 할 건물 리스트입니다." +
        "\n각 건물의 ID와 생성할 위치를 설정하세요." +
        "\n생성할 위치는 그리드 좌표로 설정합니다." +
        "\nSpawnInterval은 각 건물이 생성될 때 시간 간격입니다."
    )]
    [SerializeField]
    SpawnList[] spawnList;

    [SerializeField] private float spawnInterval = 0.1f;

    public static bool OnEnemyCollsionIgnore => _onEnemyCollsionIgnore;

    private void Start()
    {
        _onEnemyCollsionIgnore = false;
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
            _onEnemyCollsionIgnore = !_onEnemyCollsionIgnore;
            Debug.Log($"On Enemy Collision Ignore: {_onEnemyCollsionIgnore}");
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
}
#endif