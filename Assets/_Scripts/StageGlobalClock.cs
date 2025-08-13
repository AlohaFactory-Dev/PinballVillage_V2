using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StageGlobalClock : MonoBehaviour
{
    private readonly ReactiveProperty<string> _formattedTime = new("00:00");
    public IReadOnlyReactiveProperty<string> FormattedTime => _formattedTime;
    public IReactiveProperty<int> CurrentTick { get; private set; } = new ReactiveProperty<int>(0);

    private int _stageEndCondition = 0;

    private class Timer
    {
        public string id;
        public float interval;
        public Action callback;
        public bool isOneShot;
        public IDisposable disposable;

        public Timer(string id, float interval, Action callback, bool isOneShot)
        {
            this.id = id;
            this.interval = interval;
            this.callback = callback;
            this.isOneShot = isOneShot;
        }
    }

    private readonly Dictionary<string, Timer> _timers = new();

    public void Init()
    {
        _stageEndCondition = (int)TableListContainer.Get<EtcTableList>().GetEtcTable("stageClearCondition").values[0];
        _formattedTime.Value = GetFormattedElapsedTime();
        IntervalWithTimeScale(1f)
            .Subscribe(_ =>
            {
                CurrentTick.Value++;
                if (CurrentTick.Value >= _stageEndCondition)
                    StageContainer.Get<StageManager>().StageResult();

                _formattedTime.Value = GetFormattedElapsedTime();
            }).AddTo(this);
    }

    private IObservable<long> IntervalWithTimeScale(float intervalSeconds)
    {
        return Observable.Create<long>(observer =>
        {
            float elapsed = 0f;
            long count = 0;
            return Observable.EveryUpdate().Subscribe(_ =>
            {
                elapsed += Time.deltaTime;
                if (elapsed >= intervalSeconds)
                {
                    elapsed -= intervalSeconds;
                    observer.OnNext(count++);
                }
            });
        });
    }

    private string GetFormattedElapsedTime()
    {
        int elapsed = _stageEndCondition - CurrentTick.Value;
        int minutes = elapsed / 60;
        int seconds = elapsed % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    public bool RegisterRepeatingTimer(string id, float interval, Action callback, bool isOneShot = false)
    {
        if (_timers.ContainsKey(id))
        {
            Debug.LogWarning($"StageGlobalClock: Timer with ID '{id}' already exists");
            return false;
        }

        var timer = new Timer(id, interval, callback, isOneShot);
        timer.disposable = CurrentTick
            .Skip(1)
            .Where(tick => tick % Mathf.RoundToInt(interval) == 0)
            .Subscribe(_ =>
            {
                timer.callback?.Invoke();
                if (timer.isOneShot)
                    UnregisterTimer(timer.id);
            });

        _timers.Add(id, timer);
        return true;
    }

    public bool RegisterTimer(string id, float interval, Action callback)
        => RegisterRepeatingTimer(id, interval, callback, true);

    public void UnregisterTimer(string id)
    {
        if (_timers.TryGetValue(id, out var timer))
        {
            timer.disposable?.Dispose();
            _timers.Remove(id);
            Debug.Log($"StageGlobalClock: Unregistered timer '{id}'");
        }
    }

    private bool HasTimer(string id) => _timers.ContainsKey(id);
}