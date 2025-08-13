using System;
using System.Collections.Generic;
using FactorySystem;
using UniRx;
using UnityEngine;

/// <summary>
/// 전역 시계 시스템 - 여러 오브젝트가 서로 다른 주기로 동작할 수 있도록 관리
/// </summary>
public class StageGlobalClock : MonoBehaviour
{
    private ReactiveProperty<string> _formattedTime = new ReactiveProperty<string>("00:00");
    public IReadOnlyReactiveProperty<string> FormattedTime => _formattedTime;
    public IReactiveProperty<int> CurrentTick { get; private set; } = new ReactiveProperty<int>(0);
    private int _stageEndCondition = 0; // 스테이지 종료 조건 (예: 60초 후 종료 등)

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

    private List<Timer> timers = new List<Timer>();


    void Awake()
    {
        _stageEndCondition = (int)TableListContainer.Get<EtcTableList>().GetEtcTable("stageClearCondition").values[0];
        _formattedTime.Value = GetFormattedElapsedTime();
        IntervalWithTimeScale(1f)
            .Subscribe(_ =>
            {
                CurrentTick.Value++;
                if (CurrentTick.Value >= _stageEndCondition)
                {
                    StageContainer.Get<StageManager>().StageResult();
                }

                _formattedTime.Value = GetFormattedElapsedTime();
            })
            .AddTo(this);
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
        if (HasTimer(id))
        {
            Debug.LogWarning($"StageGlobalClock: Timer with ID '{id}' already exists");
            return false;
        }

        var timer = new Timer(id, interval, callback, isOneShot);

        // CurrentTick을 구독하여 전체 시간에서 interval마다 콜백 실행
        var obs = CurrentTick
            .Skip(1) // 0초는 제외
            .Where(tick => tick % Mathf.RoundToInt(interval) == 0)
            .Subscribe(_ =>
            {
                timer.callback?.Invoke();
                if (timer.isOneShot)
                {
                    UnregisterTimer(timer.id);
                }
            });

        timer.disposable = obs;
        timers.Add(timer);

        // Debug.Log($"StageGlobalClock: Registered repeating timer '{id}' with interval {interval}s");
        return true;
    }

    public bool RegisterTimer(string id, float interval, Action callback)
    {
        return RegisterRepeatingTimer(id, interval, callback, true);
    }

    public bool UnregisterTimer(string id)
    {
        Timer timerToRemove = null;
        foreach (Timer timer in timers)
        {
            if (timer.id == id)
            {
                timerToRemove = timer;
                break;
            }
        }

        if (timerToRemove != null)
        {
            timerToRemove.disposable?.Dispose();
            timers.Remove(timerToRemove);
            Debug.Log($"StageGlobalClock: Unregistered timer '{id}'");
            return true;
        }

        return false;
    }

    public bool HasTimer(string id)
    {
        foreach (Timer timer in timers)
        {
            if (timer.id == id)
                return true;
        }

        return false;
    }
}