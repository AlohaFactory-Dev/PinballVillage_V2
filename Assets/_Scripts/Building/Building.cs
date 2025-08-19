using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UniRx;
using Zenject;
using FactorySystem;
using Aloha.Coconut;


[RequireComponent(typeof(RecycleObject))]
[RequireComponent(typeof(BuildingHp))]
public abstract class Building : MonoBehaviour
{
    // ===== [DI] =====
    [Inject] private StageUI _stageUI;
    [Inject] private BuildingManager _buildingManager;
    [Inject] private StageGlobalClock _stageGlobalClock;
    [Inject] private FactoryManager _factoryManager;

    // ===== [Serialized Fields] =====
    [SerializeField] private float lordActionInterval = 0.15f;
    [SerializeField] private GameObject buildingObj;
    [SerializeField] private GameObject rubbleObj;

    // ===== [Private Fields] =====
    private BuildingFloatingTextPoint _floatingTextPoint;
    private SortingGroup _sortingGroup;
    private readonly ReactiveProperty<bool> _isDestroyed = new(false);
    private BuildingAnimationSystem _animationSystem;
    private string _timerId;
    private RecycleObject _recycleObject;
    private Passive _passive;
    private BuildingHp _buildingHp;
    private BuildingInfoPopupPoint _popupPoint;
    private bool _hasPassive;
    private bool _hasComponents;
    private bool _hasHp;

    // ===== [Protected Fields] =====
    protected Collider2D Collider2D;
    protected BuildingFunction BuildingFunction;

    // ===== [Public Properties] =====
    public Spawner Spawner { get; private set; }
    public bool HasHp => _hasHp;
    public OwnerType OwnerType => Spawner.CurrentOwner;
    public BuildingTable Table { get; private set; }
    public BuildingGroupType GroupType => Table.group;
    public int RestoreCost { get; private set; }
    public bool IsMaxLevel => Table.level == Table.maxLevel;
    public BuildingFloatingTextPoint FloatingTextPoint => _floatingTextPoint;
    public IReadOnlyReactiveProperty<bool> IsDestroyedReadOnly => _isDestroyed;

    // ===== [Init] =====
    public virtual void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        Spawner = spawner;
        Table = table;
        GetComponents();

        RestoreCost = Mathf.CeilToInt(table.buildCost * TableListContainer.Get<EtcTableList>().GetEtcTable("restoreCost").values[0]);
        _hasPassive = false;
        Collider2D.enabled = true;
        buildingObj.SetActive(true);
        rubbleObj.SetActive(false);

        _hasHp = _buildingHp.HasBar;
        _isDestroyed.Value = false;
        if (_hasHp) _buildingHp.Init(table.maxHp);


        transform.position = spawner.transform.position;
        if (_animationSystem) _animationSystem.Init();


        if (table.triggerTiming != TriggerTiming.None)
        {
            BuildingFunction = GetComponent<BuildingFunction>();
            BuildingFunction.Init(table, spawner, this);
        }

        spawner.SetBuilding(this);

        switch (Table.triggerTiming)
        {
            case TriggerTiming.Auto:
                _timerId = $"{Table.id}_{GetInstanceID()}";
                StartTimer();
                break;
            case TriggerTiming.OnSpawn:
                _animationSystem.SetOnSpawnEvent(() => PerformAction(null));
                break;
        }

        _hasPassive = !TableManager.IsMagicNumber(Table.passiveId);
        if (_hasPassive)
        {
            _passive = BuildingPassiveContainer.GetPassive(Table.passiveId);
            _passive.Init(spawner, Table.passiveId);
        }

        _sortingGroup.sortingOrder = spawner.GridPosition.x + spawner.GridPosition.y * 100;
    }

    // ===== [Component Getter] =====
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
        _floatingTextPoint = GetComponentInChildren<BuildingFloatingTextPoint>(true);
    }

    // ===== [Timer] =====
    private void StartTimer()
    {
        _stageGlobalClock.RegisterRepeatingTimer(_timerId, Table.interval, () =>
        {
            if (OwnerType == OwnerType.Player)
            {
                _animationSystem.Activate();
                PerformAction(null);
            }
        });
        // Debug.Log($"{_timerId} started with {Table.interval}s interval");
    }

    // ===== [Collision] =====
    protected void OnCollisionExit2D(Collision2D other)
    {
        if (Table.triggerTiming != TriggerTiming.OnCollision) return;
        if (other.gameObject.TryGetComponent(out Villager villager))
        {
            OnCollisionFunction(villager);
        }
    }

    public void OnCollisionFunction(IChanger changer)
    {
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

    // ===== [Damage & HP] =====
    public void TakeDamage(int attackPower)
    {
        _animationSystem.TakeDamage();
        _floatingTextPoint.ShowDamageText(attackPower);
        if (_hasHp && _buildingHp.TakeDamage(attackPower))
        {
            Destroy();
        }
    }

    public void RecoverHp(float value)
    {
        if (_hasHp) _buildingHp.Recover(value);
    }

    // ===== [Action] =====
    protected abstract void PerformAction(IChanger changer);

    // ===== [Destroy/Restore] =====
    private void Destroy()
    {
        if (!_hasHp || OwnerType == OwnerType.Enemy)
        {
            _buildingManager.RemoveBuilding(this);
        }

        if (GroupType == BuildingGroupType.Castle)
        {
            _buildingManager.RemoveCastle(this);
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

        RemovePassive();
    }

    public void Restore()
    {
        Init(Table, Spawner, false);
        if (GroupType == BuildingGroupType.Castle)
        {
            _buildingManager.RestoreCastle(this);
        }
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

        RemovePassive();
        Spawner.ClearBuilding();
        _recycleObject.Release();
    }

    private void RemovePassive()
    {
        if (_hasPassive)
        {
            _passive.RemovePassive();
        }
    }

    // ===== [Performance] =====
    public void UpdatePerformance(Passive passive)
    {
        if (GroupType != passive.TargetGroupType) return;
        BuildingFunction.UpgradePerformance(passive);
    }

    public void DowngradePerformance(Passive passive)
    {
        if (GroupType != passive.TargetGroupType) return;
        BuildingFunction.DowngradePerformance(passive);
    }

    // ===== [UI] =====
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