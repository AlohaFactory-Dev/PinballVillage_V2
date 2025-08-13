using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class GoldText : MonoBehaviour
{
    private int _actionTrigger;
    [Inject] private GoldManager _goldManager;
    private Animator _animator;
    [SerializeField] private TextMeshProUGUI goldText;
    private int _lastGoldValue = 0;
    private Tween _goldTween;
    private float _goldTweenDuration = 0.1f; // 애니메이션 지속 시간

    public void Init()
    {
        _actionTrigger = Animator.StringToHash("Action");
        _animator = GetComponent<Animator>();
        _goldManager.GoldAmount.Subscribe(UpdateGoldText).AddTo(this);
        _lastGoldValue = _goldManager.GoldAmount.Value;

        UpdateGoldText(_goldManager.GoldAmount.Value);
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
            }, endValue, _goldTweenDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                goldText.text = $"{endValue}";
                _lastGoldValue = endValue;
            }).Play();
        if (_lastGoldValue < value)
        {
            _animator.SetTrigger(_actionTrigger);
        }
    }
}