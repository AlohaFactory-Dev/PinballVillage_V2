using Aloha.CoconutMilk;
using UnityEngine;

public class BuildingHp : MonoBehaviour
{
    [SerializeField] private BarGauge hpBar;
    private Animator _animator;
    public bool HasBar => hpBar != null;
    private int _defaultMaxHp;
    private int _currentHp;

    public void Init(int maxHp)
    {
        _defaultMaxHp = maxHp;
        _currentHp = maxHp;
        hpBar.Init(maxHp, null, maxHp);
        hpBar.Off();
        _animator = GetComponentInChildren<Animator>();
    }


    public bool TakeDamage(int amount)
    {
        _currentHp -= amount;
        _animator.SetTrigger("Damage");
        hpBar.SetValue(_currentHp);
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.On();
        }

        if (_currentHp <= 0)
        {
            _currentHp = 0;
            hpBar.Off();
        }

        return _currentHp <= 0;
    }

    public void Recover(float value)
    {
        if (_currentHp >= _defaultMaxHp)
        {
            return; // 이미 최대 체력에 도달했으면 회복하지 않음
        }

        _currentHp += Mathf.CeilToInt(_defaultMaxHp * value);
        if (_currentHp > _defaultMaxHp)
        {
            _currentHp = _defaultMaxHp;
        }

        hpBar.SetValue(_currentHp);
        if (_currentHp >= _defaultMaxHp)
        {
            hpBar.Off();
        }
    }
}