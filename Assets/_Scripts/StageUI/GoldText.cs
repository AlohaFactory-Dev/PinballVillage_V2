using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class GoldText : MonoBehaviour
{
    public enum GoldTextType
    {
        None,
        Second,
        Minute
    }

    private int _actionTrigger;
    [Inject] private GoldManager _goldManager;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Animator goldTextAnimator;
    [SerializeField] private TextMeshProUGUI goldPerSecondText;
    [SerializeField] private GoldTextType goldTextType = GoldTextType.Second;
    private int _lastGoldValue = 0;
    private Tween _goldTween;
    [SerializeField] private float goldTextTweenDuration = 0.1f; // 애니메이션 지속 시간

    [SerializeField] private int decimalPlaces = 2; // Inspector에서 소수점 자리수 조절

    public void Init()
    {
        _actionTrigger = Animator.StringToHash("Action");
        _goldManager.GoldAmount.Subscribe(UpdateGoldText).AddTo(this);
        _lastGoldValue = _goldManager.GoldAmount.Value;

        UpdateGoldText(_goldManager.GoldAmount.Value);
        if (goldTextType == GoldTextType.None)
        {
            goldPerSecondText.gameObject.SetActive(false);
        }
        else
        {
            goldPerSecondText.gameObject.SetActive(true);
            if (goldTextType == GoldTextType.Minute)
            {
                _goldManager.GoldPerMinute.Subscribe(UpdateGoldPerSecondText).AddTo(this);
                UpdateGoldPerSecondText(_goldManager.GoldPerMinute.Value);
            }
            else
            {
                _goldManager.GoldPerSecond.Subscribe(UpdateGoldPerSecondText).AddTo(this);
                UpdateGoldPerSecondText(_goldManager.GoldPerSecond.Value);
            }
        }
    }

    private void UpdateGoldText(int value)
    {
        int startValue = _lastGoldValue;
        int endValue = value;
        _goldTween?.Kill();
        _goldTween = DOTween.To(() => startValue, x =>
            {
                goldText.text = $"{x}";
                startValue = x;
            }, endValue, goldTextTweenDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                goldText.text = $"{endValue}";
                _lastGoldValue = endValue;
            }).Play();
        if (_lastGoldValue < value)
        {
            goldTextAnimator.SetTrigger(_actionTrigger);
        }
    }

    private void UpdateGoldPerSecondText(float value)
    {
        string format = $"F{decimalPlaces}";
        if (goldTextType == GoldTextType.Minute)
        {
            goldPerSecondText.text = $"{value.ToString(format)}/m";
            return;
        }

        goldPerSecondText.text = $"{value.ToString(format)}/s";
    }
}