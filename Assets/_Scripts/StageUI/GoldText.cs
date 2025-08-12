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
        if (_lastGoldValue < value)
        {
            _animator.SetTrigger(_actionTrigger);
        }

        goldText.text = $"{value}";
        _lastGoldValue = value;
    }
}