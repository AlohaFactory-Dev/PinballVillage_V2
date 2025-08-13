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

public class TestManager : MonoBehaviour
{
    [InfoBox("G : 10000골드 추가" +
        "\nL : 모든 건물 레벨업" +
        "\nUpArrow : Time Scale + 1" +
        "\nDownArrow : Time Scale - 1" +
        "\nRightArrow, LeftArrow : Time Scale = 1")]
    [Inject]
    GoldManager goldManager;
# if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            goldManager.AddGold(10000);
        if (Input.GetKeyDown(KeyCode.L))
        {
            StageContainer.Get<BuildingManager>().PlayerAllBuildingLevelUp();
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
#endif
}