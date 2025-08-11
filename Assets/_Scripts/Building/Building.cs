using System.Collections;
using FactorySystem;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;


[RequireComponent(typeof(RecycleObject))]
[RequireComponent(typeof(BuildingHp))]
public class Building : MonoBehaviour
{
    public enum CalculateType
    {
        Add,
        Multiply,
    }

    [Inject] private StageUI _stageUI;
    [Inject] private BuildingManager _buildingManager;
    [SerializeField] private float lordActionInterval = 0.15f;
    [SerializeField] GameObject buildingObj;
    [SerializeField] GameObject rubbleObj;


    protected ReactiveProperty<bool> IsDestroyed = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsDestroyedReadOnly => IsDestroyed;
    private BuildingAnimationSystem _animationSystem;
    private string _timerId;
    private RecycleObject _recycleObject;
    private Passive _passive;

    public bool IsMaxLevel => Table.level == Table.maxLevel;
    protected Collider2D Collider2D;
    public Spawner Spawner { get; private set; }
    protected BuildingFunction BuildingFunction;

    [Inject] private StageGlobalClock _stageGlobalClock;
    private BuildingHp _buildingHp;
    private bool _hasHp;

    public bool HasHp => _hasHp;
    public OwnerType OwnerType => Spawner.CurrentOwner;
    public BuildingTable Table { get; private set; }
    public BuildingGroupType PassiveTargetGroupType => Table.passiveTargetGroup;
    public int RestroeCost => Mathf.CeilToInt(Table.buildCost * TableListContainer.Get<EtcTableList>().GetEtcTable("restoreCost").values[0]);
    private BuildingInfoPopupPoint _popupPoint;

    public virtual void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        _popupPoint = GetComponentInChildren<BuildingInfoPopupPoint>(true);
        _recycleObject = GetComponent<RecycleObject>();
        Collider2D = GetComponentInChildren<Collider2D>(true);
        _animationSystem = GetComponentInChildren<BuildingAnimationSystem>(true);
        _buildingHp = GetComponent<BuildingHp>();

        Collider2D.enabled = true;
        buildingObj.SetActive(true);
        rubbleObj.SetActive(false);

        _hasHp = _buildingHp.HasBar;
        IsDestroyed.Value = false;
        if (_hasHp)
        {
            _buildingHp.Init(table.maxHp);
        }

        Spawner = spawner;
        Table = table;
        transform.position = spawner.transform.position;
        if (_animationSystem)
        {
            _animationSystem.Init();
        }

        if (table.triggerTiming != TriggerTiming.None)
        {
            BuildingFunction = GetComponent<BuildingFunction>();
            BuildingFunction.Init(table, spawner);
        }

        switch (Table.triggerTiming)
        {
            case TriggerTiming.Auto:
                _timerId = $"{Table.id}_{GetInstanceID()}";
                StartTimer();
                break;
            case TriggerTiming.OnSpawn:
                _animationSystem.SetOnSpawnEvent(() => PerformAction());
                break;
        }

        _passive = GetComponent<Passive>();
        if (_passive)
        {
            _passive.AddPassive(spawner);
        }

        spawner.SetBuilding(this);
    }

    private void StartTimer()
    {
        if (_stageGlobalClock.RegisterRepeatingTimer(_timerId, Table.interval, () =>
            {
                if (OwnerType == OwnerType.Player)
                {
                    _animationSystem.Activate();
                    PerformAction();
                }
            }))
        {
            Debug.Log($"{_timerId} started with {Table.interval}s interval");
        }
    }


    protected void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Villager villager))
        {
            OnCollisionFunction(villager);
        }
    }

    public void TakeDamage(int attackPower)
    {
        _animationSystem.TakeDamage();
        if (_hasHp)
        {
            if (_buildingHp.TakeDamage(attackPower))
            {
                // 건물이 파괴되면 제거
                Destroy();
            }
        }
    }

    public void RecoverHp(float value)
    {
        if (_hasHp)
        {
            _buildingHp.Recover(value);
        }
    }

    public void OnCollisionFunction(IChanger changer)
    {
        if (Table.triggerTiming != TriggerTiming.OnCollision)
            return;

        // 중립 또는 소유자 일치 시 처리
        if (changer.OwnerType == OwnerType || OwnerType == OwnerType.Neutral)
        {
            if (changer.VillagerType == VillagerType.Lord)
            {
                StartCoroutine(LordMultiplierCoroutine(changer));
            }
            else
            {
                _animationSystem.Activate();
                PerformAction(changer);
            }
        }
    }

    private IEnumerator LordMultiplierCoroutine(IChanger changer)
    {
        _animationSystem.Activate();
        PerformAction(changer);
        yield return new WaitForSeconds(lordActionInterval);
        _animationSystem.Activate();
        PerformAction(changer);
    }

    protected virtual void PerformAction(IChanger changer = null, int value = 0, CalculateType calculate = CalculateType.Add)
    {
        BuildingFunction.PerformAction(changer, value, calculate);
    }

    private void Destroy()
    {
        if (!_hasHp || OwnerType == OwnerType.Enemy)
        {
            _buildingManager.RemoveBuilding(this);
        }

        if (Table.triggerTiming != TriggerTiming.None)
        {
            BuildingFunction.DestroyAction();
        }

        IsDestroyed.Value = true;
        Collider2D.enabled = false;
        rubbleObj.SetActive(true);
        buildingObj.SetActive(false);
        if (Table.triggerTiming == TriggerTiming.Auto)
        {
            _stageGlobalClock.UnregisterTimer(_timerId);
        }

        if (_passive)
        {
            _passive.RemovePassive();
        }
    }

    public void Restore()
    {
        Init(Table, Spawner, false);
    }

    public void RemoveBuilding()
    {
        if (Table.triggerTiming == TriggerTiming.Auto)
        {
            _stageGlobalClock.UnregisterTimer(_timerId);
        }

        if (Table.triggerTiming != TriggerTiming.None)
        {
            BuildingFunction.DestroyAction();
        }

        if (_passive)
        {
            _passive.RemovePassive();
        }

        Spawner.ClearBuilding();
        _recycleObject.Release();
    }

    public void UpdatePerformance(float value)
    {
        BuildingFunction.UpgradePerformance(value);
    }

    public void OpenInfo()
    {
        BuildingInfoPopup.Args args = new BuildingInfoPopup.Args
        {
            Position = _popupPoint.GetPoint(),
            OpenType = BuildingPopupOpenType.Building,
            Building = this,
            Table = Table,
        };
        _stageUI.OpenPopup(StageUI.PopupConfig.BuildingInfoPopupConfig, args);
    }
}