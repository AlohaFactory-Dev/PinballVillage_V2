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

    [Inject] private StageGlobalClock _stageGlobalClock;
    [Inject] private CoconutCanvas _coconutCanvas;
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [SerializeField] private GoldText goldText;
    [SerializeField] private GameObject stageResultPanel;
    [SerializeField] private GameObject failText;
    [SerializeField] private GameObject winText;
    [SerializeField] private Button restartButton;
    [SerializeField] private BarGauge stagePercentGauge;
    [SerializeField] private RectTransform conditionRect;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float resultPanelDelay = 1f;
    private BuildingCardContainer _buildingCardContainer;

    public BuildingCardContainer BuildingCardContainer => _buildingCardContainer;

    public void Start()
    {
        _buildingCardContainer = GetComponentInChildren<BuildingCardContainer>(true);
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
        goldText.Init();
    }

    public void OnStageResult(OwnerType owner)
    {
        StartCoroutine(StageResultCo(owner));
    }

    IEnumerator StageResultCo(OwnerType owner)
    {
        yield return new WaitForSecondsRealtime(resultPanelDelay);
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


    private void UpdateTimerText(string timeText)
    {
        timerText.text = timeText;
    }
# if UNITY_EDITOR
    public void On()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void Off()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
    }
#endif
}