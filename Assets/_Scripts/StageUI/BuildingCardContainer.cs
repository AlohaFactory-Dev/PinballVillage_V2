using Aloha.Coconut;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class BuildingCardContainer : MonoBehaviour
{
    [Inject] private BuildModeManager _buildModeManager;
    [Inject] private GoldManager _goldManager;
    [SerializeField] private Button refreshButton;
    [SerializeField] private TextMeshProUGUI refreshGoldText;
    [SerializeField] private GameObject refreshTicketFrame;
    [SerializeField] private TextMeshProUGUI refreshTicketText;
    private BuildingCard[] _buildingCards;
    private int _requiredGoldToRefresh = 100;
    private int _addedGold = 0;
    private bool lastRefreshByMouseUp = false; // 추가

    public void Init()
    {
        _buildingCards = GetComponentsInChildren<BuildingCard>(true);
        var etc = TableListContainer.Get<EtcTableList>().GetEtcTable("buildingCardRefreshCount");
        _requiredGoldToRefresh = (int)etc.values[0];
        _addedGold = (int)etc.values[1];
        foreach (var card in _buildingCards)
        {
            card.Init();
        }

        refreshButton.onClick.AddListener(RefreshCards);

        _goldManager.GoldAmount
            .Subscribe(_ => UpdateRefeshButtonInteractable())
            .AddTo(this);
        _goldManager.RefreshTicketAmount
            .Subscribe(_ => UpdateRefeshButtonInteractable())
            .AddTo(this);
        UpdateRefeshButtonInteractable();
    }

    private void UpdateRefeshButtonInteractable()
    {
        refreshTicketFrame.SetActive(_goldManager.HasRefreshTicket);
        if (_goldManager.HasRefreshTicket)
        {
            refreshGoldText.text = TextTableV2.Get("Common/Free");
            refreshGoldText.color = Color.white;
            refreshTicketText.text = $"{_goldManager.RefreshTicketAmount.Value}";
        }
        else
        {
            refreshGoldText.text = $"<sprite name=Gold>{_requiredGoldToRefresh}";
            refreshGoldText.color = _goldManager.EnoughGold(_requiredGoldToRefresh)
                ? Color.white
                : Color.red;
        }
    }

    public void RefreshCards()
    {
        lastRefreshByMouseUp = Input.GetMouseButtonUp(0); // 마우스 업으로 실행됐는지 기록
        if (_goldManager.HasRefreshTicket)
        {
            foreach (var card in _buildingCards)
            {
                card.Refesh();
            }

            _goldManager.UseRefreshTicket();

            return;
        }

        if (_goldManager.EnoughGold(_requiredGoldToRefresh))
        {
            foreach (var card in _buildingCards)
            {
                card.Refesh();
            }

            int usedGold = _requiredGoldToRefresh;
            _requiredGoldToRefresh += _addedGold; // 다음 리프레시 비용 증가
            _goldManager.UseGold(usedGold);
        }
    }

    // TestManager에서 호출: 최근 RefreshCards가 마우스 업으로 실행됐는지 확인
    public bool WasLastRefreshByMouseUp()
    {
        bool result = lastRefreshByMouseUp;
        lastRefreshByMouseUp = false; // 한 번 확인 후 초기화
        return result;
    }
}