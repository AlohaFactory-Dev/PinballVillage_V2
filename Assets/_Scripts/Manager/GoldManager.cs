using Aloha.Coconut;
using UniRx;
using UnityEngine;

public class GoldManager
{
    private readonly ReactiveProperty<int> _goldAmount = new(0);
    public IReadOnlyReactiveProperty<int> GoldAmount => _goldAmount;
    private readonly ReactiveProperty<int> _refreshTicketAmount = new(0);
    public IReadOnlyReactiveProperty<int> RefreshTicketAmount => _refreshTicketAmount;
    public bool HasRefreshTicket => _refreshTicketAmount.Value > 0;

    public GoldManager(StageGlobalClock stageGlobalClock)
    {
        var etcTableList = TableListContainer.Get<EtcTableList>().GetEtcTable("initialGold");
        int initialGold = (int)etcTableList.values[0];
        int initialRefreshTicket = (int)etcTableList.values[1];
        AddGold(initialGold);
        AddRefreshTicket(initialRefreshTicket);
        stageGlobalClock.RegisterRepeatingTimer("refreshTicket", etcTableList.values[2], () => { AddRefreshTicket((int)etcTableList.values[3]); });
    }


    private void AddRefreshTicket(int amount)
    {
        _refreshTicketAmount.Value += amount;
    }

    public void UseRefreshTicket()
    {
        if (_refreshTicketAmount.Value > 0)
        {
            _refreshTicketAmount.Value--;
        }
    }

    public void AddGold(int amount)
    {
        _goldAmount.Value += amount;
    }

    public void UseGold(int amount)
    {
        if (_goldAmount.Value >= amount)
        {
            _goldAmount.Value -= amount;
        }
    }

    public bool EnoughGold(int amount)
    {
        bool enough = _goldAmount.Value >= amount;

        return enough;
    }
}