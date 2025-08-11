using System;
using UnityEngine;

public class BuildingAnimationSystem : MonoBehaviour
{
    private Animator _animator;

    private int _spawnTrigger;
    private int _activateTrigger;
    private int _damageTrigger;

    private Action _onSpawnEvent;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _activateTrigger = Animator.StringToHash("Activate");
        _spawnTrigger = Animator.StringToHash("Spawn");
        _damageTrigger = Animator.StringToHash("Hit");
        _animator.SetTrigger(_spawnTrigger);
    }

    public void SetOnSpawnEvent(Action onSpawnEvent)
    {
        _onSpawnEvent = onSpawnEvent;
    }

    // <summary>
    // 애니메이션 이벤트에서 호출되는 메서드
    // </summary>
    public void OnSpawnEvent()
    {
        _onSpawnEvent?.Invoke();
    }

    public void Activate()
    {
        _animator.SetTrigger(_activateTrigger);
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(_damageTrigger);
    }
}