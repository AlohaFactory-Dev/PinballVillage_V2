using Aloha.Coconut;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class GoldManager
{
    private readonly ReactiveProperty<int> _goldAmount = new(0);
    public IReadOnlyReactiveProperty<int> GoldAmount => _goldAmount;
    private readonly ReactiveProperty<int> _refreshTicketAmount = new(0);
    public IReadOnlyReactiveProperty<int> RefreshTicketAmount => _refreshTicketAmount;
    public bool HasRefreshTicket => _refreshTicketAmount.Value > 0;

    // 골드 획득 기록 (tick, amount)
    private readonly Queue<(int tick, int amount)> _goldGainHistory = new();
    private int _lastTick = 0;
    private int _lastGoldAmount = 0;

    private readonly ReactiveProperty<float> _goldPerSecond = new(0f);
    public IReadOnlyReactiveProperty<float> GoldPerSecond => _goldPerSecond;
    private readonly ReactiveProperty<float> _goldPerMinute = new(0f);
    public IReadOnlyReactiveProperty<float> GoldPerMinute => _goldPerMinute;

    public GoldManager(StageGlobalClock stageGlobalClock)
    {
        var etcTableList = TableListContainer.Get<EtcTableList>().GetEtcTable("initialGold");
        int initialGold = (int)etcTableList.values[0];
        int initialRefreshTicket = (int)etcTableList.values[1];
        AddGold(initialGold);
        AddRefreshTicket(initialRefreshTicket);
        stageGlobalClock.RegisterRepeatingTimer("refreshTicket", etcTableList.values[2], () => { AddRefreshTicket((int)etcTableList.values[3]); });

        _lastTick = stageGlobalClock.CurrentTick.Value;
        _lastGoldAmount = _goldAmount.Value;

        // 매초 골드 획득량 계산
        stageGlobalClock.CurrentTick
            .Subscribe(OnTickChanged)
            .AddTo(stageGlobalClock);
    }

    private void OnTickChanged(int tick)
    {
        int gained = _goldAmount.Value - _lastGoldAmount;
        if (gained > 0)
        {
            _goldGainHistory.Enqueue((tick, gained));
        }

        _lastTick = tick;
        _lastGoldAmount = _goldAmount.Value;

        // 60초(틱) 이내 기록만 유지
        while (_goldGainHistory.Count > 0 && _goldGainHistory.Peek().tick < tick - 59)
        {
            _goldGainHistory.Dequeue();
        }

        // 초당 골드 획득량 계산
        int totalGain = 0;
        foreach (var entry in _goldGainHistory)
            totalGain += entry.amount;
        float perSecond = _goldGainHistory.Count > 0 ? totalGain / (float)_goldGainHistory.Count : 0f;
        _goldPerSecond.Value = perSecond;
        _goldPerMinute.Value = perSecond * 60f;
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