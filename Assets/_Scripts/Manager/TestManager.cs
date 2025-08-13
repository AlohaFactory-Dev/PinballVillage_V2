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
    "\nRightArrow, LeftArrow : Time Scale = 1")]
public class TestManager : MonoBehaviour
{
    [Inject] private GoldManager goldManager;

    [InfoBox("모든 건물 레벨업을 할지, 특정 건물만 레벨업할지 선택하세요.")]
    [SerializeField]
    private bool allBuildingLevelUp = false;

    [SerializeField] private List<string> levelUpBuildingIds = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            goldManager.AddGold(10000);
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