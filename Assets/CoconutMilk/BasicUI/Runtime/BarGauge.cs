using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Aloha.CoconutMilk
{
    public class BarGauge : MonoBehaviour
    {
        public float MaxValue => maxValue;
        public float Value => value;

        [SerializeField] private SlicedFilledImage fillImage;
        [SerializeField] private TMP_Text fillText;
        [SerializeField] private float duration = .2f;
        [SerializeField] private AnimationCurve curve;

        [InfoBox("디버그용으로만 에디터에서 조정, 실제로 두 값은 Initialize와 SetValue로 설정되어야 함")]
        [SerializeField]
        [OnValueChanged(nameof(EditorOnValueChanged))]
        private float value;

        [SerializeField]
        [OnValueChanged(nameof(EditorOnValueChanged))]
        private float maxValue;

        private Func<float, float, string> _stringFormatter;
        private bool _onfillText;
        private Tween _fillTween;

        private bool _onLevelUpAnimation;

        public void Init(float maxValue, Func<float, float, string> stringFormatter = null, float initialValue = 0)
        {
            this.maxValue = maxValue;
            _stringFormatter = stringFormatter;
            _onfillText = stringFormatter != null;
            fillText.enabled = _onfillText;
            SetValue(initialValue);
        }

        public void On()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        public void Off()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void SetMaxValue(float maxValue)
        {
            this.maxValue = maxValue;
            fillImage.fillAmount = value / maxValue;
            if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
        }

        public void SetValue(float value, bool onAnimation = false)
        {
            if (onAnimation)
            {
                SetValueFillAnimation(value);
                return;
            }

            this.value = value;
            fillImage.fillAmount = this.value / maxValue;
            if (_onfillText) fillText.text = _stringFormatter(this.value, maxValue);
        }

        private void SetValueFillAnimation(float targetValue)
        {
            if (_fillTween != null && _fillTween.IsActive())
            {
                _fillTween.Kill();
            }

            _fillTween = DOTween.To(() => value, x => value = x, targetValue, duration)
                .SetEase(curve)
                .OnUpdate(() =>
                {
                    fillImage.fillAmount = value / maxValue;
                    if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
                });
        }

        private void EditorOnValueChanged()
        {
            fillImage.fillAmount = value / maxValue;
            if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
        }


        public void SetPercent(float percent, bool onAnimation = false)
        {
            if (onAnimation)
            {
                SetPercentFillAnimation(percent);
                return;
            }

            fillImage.fillAmount = percent;
            if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
        }

        public void LevelUp(Action levelUp, float percent)
        {
            if (_fillTween != null && _fillTween.IsActive())
            {
                _fillTween.Kill();
            }

            _onLevelUpAnimation = true;
            _fillTween = DOTween.To(() => value, x => value = x, 1, duration)
                .SetEase(curve)
                .SetUpdate(false)
                .OnUpdate(() =>
                {
                    fillImage.fillAmount = value;
                    if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
                }).OnComplete(() =>
                {
                    _onLevelUpAnimation = false;
                    levelUp.Invoke();
                    SetPercentFillAnimation(percent);
                });
        }

        private void SetPercentFillAnimation(float percent)
        {
            if (_onLevelUpAnimation)
            {
                return;
            }

            if (_fillTween != null && _fillTween.IsActive())
            {
                _fillTween.Kill();
            }

            _fillTween = DOTween.To(() => value, x => value = x, percent, duration)
                .SetEase(curve)
                .OnUpdate(() =>
                {
                    fillImage.fillAmount = value;
                    if (_onfillText) fillText.text = _stringFormatter(value, maxValue);
                });
        }
    }
}