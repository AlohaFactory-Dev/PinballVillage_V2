using System.Collections;
using Aloha.Coconut;
using FactorySystem;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;


[RequireComponent(typeof(RecycleObject))]
[RequireComponent(typeof(BuildingHp))]
public class Building : MonoBehaviour
{
    [Inject] private StageUI _stageUI;
    [Inject] private BuildingManager _buildingManager;
    [Inject] private StageGlobalClock _stageGlobalClock;

    [SerializeField] private float lordActionInterval = 0.15f;
    [SerializeField] private GameObject buildingObj;
    [SerializeField] private GameObject rubbleObj;

    private SortingGroup _sortingGroup;
    private ReactiveProperty<bool> _isDestroyed = new(false);
    public IReadOnlyReactiveProperty<bool> IsDestroyedReadOnly => _isDestroyed;

    private BuildingAnimationSystem _animationSystem;
    private string _timerId;
    private RecycleObject _recycleObject;
    private Passive _passive;
    private BuildingHp _buildingHp;
    private BuildingInfoPopupPoint _popupPoint;
    private bool _hasPassive;
    private bool _hasComponents;
    private bool _hasHp;

    protected Collider2D Collider2D;
    protected BuildingFunction BuildingFunction;

    public Spawner Spawner { get; private set; }
    public bool HasHp => _hasHp;
    public OwnerType OwnerType => Spawner.CurrentOwner;
    public BuildingTable Table { get; private set; }
    public BuildingGroupType PassiveTargetGroupType => Table.passiveTargetGroup;
    public int RestroeCost => Mathf.CeilToInt(Table.buildCost * TableListContainer.Get<EtcTableList>().GetEtcTable("restoreCost").values[0]);
    public bool IsMaxLevel => Table.level == Table.maxLevel;

    public virtual void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        GetComponents();
        _hasPassive = false;
        Collider2D.enabled = true;
        buildingObj.SetActive(true);
        rubbleObj.SetActive(false);

        _hasHp = _buildingHp.HasBar;
        _isDestroyed.Value = false;
        if (_hasHp) _buildingHp.Init(table.maxHp);

        Spawner = spawner;
        Table = table;
        transform.position = spawner.transform.position;
        if (_animationSystem) _animationSystem.Init();

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
                _animationSystem.SetOnSpawnEvent(OnSpawnPerformAction);
                break;
        }

        _hasPassive = !TableManager.IsMagicNumber(Table.passiveId);
        if (_hasPassive)
        {
            _passive = BuildingPassiveContainer.GetPassive(Table.passiveId);
            _passive.Init(spawner, Table.passiveId);
        }

        _sortingGroup.sortingOrder = spawner.GridPosition.x + spawner.GridPosition.y * 100;
        spawner.SetBuilding(this);
    }

    private void GetComponents()
    {
        if (_hasComponents) return;
        _hasComponents = true;
        _popupPoint = GetComponentInChildren<BuildingInfoPopupPoint>(true);
        _recycleObject = GetComponent<RecycleObject>();
        Collider2D = GetComponentInChildren<Collider2D>(true);
        _animationSystem = GetComponentInChildren<BuildingAnimationSystem>(true);
        _buildingHp = GetComponent<BuildingHp>();
        _sortingGroup = GetComponentInChildren<SortingGroup>();
    }

    private void StartTimer()
    {
        if (_stageGlobalClock.RegisterRepeatingTimer(_timerId, Table.interval, () =>
            {
                if (OwnerType == OwnerType.Player)
                {
                    _animationSystem.Activate();
                    AutoPerformAction();
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
        if (_hasHp && _buildingHp.TakeDamage(attackPower))
        {
            Destroy();
        }
    }

    public void RecoverHp(float value)
    {
        if (_hasHp) _buildingHp.Recover(value);
    }

    public void OnCollisionFunction(IChanger changer)
    {
        if (Table.triggerTiming != TriggerTiming.OnCollision) return;

        // 중립 또는 소유자 일치 시 처리
        if (changer.OwnerType == OwnerType || OwnerType == OwnerType.Neutral)
        {
            if (changer.VillagerType == VillagerType.Lord)
            {
                StartCoroutine(LordMultiplierCoroutine(changer));
            }
            else
            {
                OnCollisionPerformAction(changer);
            }
        }
    }

    private IEnumerator LordMultiplierCoroutine(IChanger changer)
    {
        OnCollisionPerformAction(changer);
        yield return new WaitForSeconds(lordActionInterval);
        OnCollisionPerformAction(changer);
    }

    protected virtual void OnCollisionPerformAction(IChanger changer)
    {
        if (Table.triggerTiming == TriggerTiming.OnCollision)
        {
            _animationSystem.Activate();
            PerformAction(new ActionContext(changer));
        }
    }


    protected virtual void AutoPerformAction()
    {
        if (Table.triggerTiming == TriggerTiming.Auto)
        {
            PerformAction();
        }
    }

    protected virtual void OnSpawnPerformAction()
    {
        if (Table.triggerTiming == TriggerTiming.OnSpawn)
        {
            PerformAction();
        }
    }

    protected void PerformAction(ActionContext actionContext = null)
    {
        BuildingFunction.PerformAction(actionContext);
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

        _isDestroyed.Value = true;
        Collider2D.enabled = false;
        rubbleObj.SetActive(true);
        buildingObj.SetActive(false);

        if (Table.triggerTiming == TriggerTiming.Auto)
        {
            _stageGlobalClock.UnregisterTimer(_timerId);
        }

        if (_hasPassive)
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

        if (_hasPassive)
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