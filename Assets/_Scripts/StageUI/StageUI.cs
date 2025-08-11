using System.Collections;
using System.Collections.Generic;
using Aloha.Coconut.Launcher;
using Aloha.Coconut.UI;
using Aloha.CoconutMilk;
using Stage.Building;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StageUI : MonoBehaviour
{
    public enum PopupConfig
    {
        BuildingInfoPopupConfig
    }

    [Inject] private GoldManager _goldManager;
    [Inject] private StageGlobalClock _stageGlobalClock;
    [Inject] private CoconutCanvas _coconutCanvas;
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [SerializeField] GameObject stageResultPanel;
    [SerializeField] GameObject failText;
    [SerializeField] GameObject winText;
    [SerializeField] Button restartButton;
    [SerializeField] BarGauge stagePercentGauge;
    [SerializeField] RectTransform conditionRect;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI timerText;
    private BuildingCardContainer _buildingCardContainer;

    public void Start()
    {
        _buildingCardContainer = GetComponentInChildren<BuildingCardContainer>(true);
        _goldManager.GoldAmount.Subscribe(UpdateGoldText).AddTo(this);
        UpdateGoldText(_goldManager.GoldAmount.Value);
        _stageGlobalClock.FormattedTime.Subscribe(UpdateTimerText).AddTo(this);
        _buildingCardContainer.Init();
        var stageClearPercent = TableListContainer.Get<EtcTableList>().GetEtcTable("stageClearCondition").values[1];
        float gaugeWidth = ((RectTransform)stagePercentGauge.transform).rect.width;
        float x = gaugeWidth * stageClearPercent;

        var anchoredPos = conditionRect.anchoredPosition;
        anchoredPos.x = x;
        conditionRect.anchoredPosition = anchoredPos;

        stagePercentGauge.Init(100, UpdatePercentText);
        _spawnerGridManager.PlayerPercent.Subscribe(x => stagePercentGauge.SetValue(x)).AddTo(this);
        stagePercentGauge.SetValue(_spawnerGridManager.PlayerPercent.Value);
        stageResultPanel.gameObject.SetActive(false);
        restartButton.onClick.AddListener(() => GlobalConainer.Get<GameSceneManager>().ReloadSceneAsync("Stage"));
    }

    public void OnStageResult(OwnerType owner)
    {
        if (owner == OwnerType.Player)
        {
            winText.SetActive(true);
            failText.SetActive(false);
        }
        else
        {
            winText.SetActive(false);
            failText.SetActive(true);
        }

        stageResultPanel.SetActive(true);
    }

    private string UpdatePercentText(float percent, float max)
    {
        return $"{(int)percent}%";
    }

    public UIView OpenPopup(PopupConfig config, UIOpenArgs openArgs = null)
    {
        return _coconutCanvas.Open(config.ToString(), openArgs);
    }

    private void UpdateGoldText(int goldAmount)
    {
        goldText.text = $"{goldAmount}";
    }

    private void UpdateTimerText(string timeText)
    {
        timerText.text = timeText;
    }
}