using System.Collections.Generic;
using Aloha.Coconut;
using Aloha.Coconut.UI;
using Aloha.CoconutMilk;
using Stage.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildModeManager : MonoBehaviour
{
    [Inject] private BuildingManager _buildingManager;
    [Inject] private GoldManager _goldManager;
    [Inject] private SpawnerGridManager _spawnerGridManager;
    [SerializeField] private DraggingBuildingPool draggingBuildingPool;

    public bool IsBuildMode { get; private set; }
    private Camera _camera;
    private LayerMask _spawnerLayerMask;
    private BuildingCard _buildingCard;
    private DraggingBuilding _draggingBuilding;
    private Spawner _currentPointerSpawner;
    private List<Spawner> _showRangeSpawners = new();

    private void Start()
    {
        _camera = Camera.main;
        draggingBuildingPool.Init();
        _spawnerLayerMask =
            (1 << LayerMask.NameToLayer("PlayerSpawner")) |
            (1 << LayerMask.NameToLayer("EnemySpawner")) |
            (1 << LayerMask.NameToLayer("NeutralSpawner"));
    }

    public void EnterBuildMode(BuildingCard buildingCard)
    {
        _buildingCard = buildingCard;
        _draggingBuilding = draggingBuildingPool.Get();
        _draggingBuilding.Set(_buildingCard);
        IsBuildMode = true;
    }

    public void ExitBuildMode(bool isCancel = false)
    {
        if (_buildingCard != null)
        {
            if (!isCancel) _buildingCard.Draw();
            else _buildingCard.DragEnd();
        }

        draggingBuildingPool.Release(_draggingBuilding);
        _currentPointerSpawner = null;
        IsBuildMode = false;
    }

    private void Update()
    {
        if (IsBuildMode)
        {
            var pointerPosition = _draggingBuilding.OnDrag(Input.mousePosition);
            if (!IsPointerOverBuildingCard())
            {
                OnDragging(pointerPosition);
            }
            else
            {
                OnDragging(new Vector2(99999, 99999));
            }

            if (Input.GetMouseButtonUp(0))
                DragEnd();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouch();
        }
    }

    private bool IsPointerOverBuildingCard()
    {
        var pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        foreach (var result in results)
        {
            if (result.gameObject.GetComponentInParent<BuildingCard>())
                return true;
        }

        return false;
    }

    private void OnDragging(Vector2 iconPoint)
    {
        Vector3 worldPosition = GetWorldPosition(iconPoint);
        var raySpawner = GetSpawnerAtPosition(worldPosition);

        if (raySpawner)
        {
            if (_currentPointerSpawner != raySpawner || !_currentPointerSpawner)
            {
                SetShowRangeSpawners(_showRangeSpawners, false);
                _currentPointerSpawner?.AllBuildAbleHighlightOff();
                _currentPointerSpawner = raySpawner;

                SetShowRangeSpawners(_spawnerGridManager.GetNeighbors(raySpawner, _buildingCard.Table.showRange), true);
                _currentPointerSpawner.BuildAbleHighlight(_currentPointerSpawner.IsEmpty && _currentPointerSpawner.CurrentOwner == OwnerType.Player);
            }
        }
        else if (_currentPointerSpawner)
        {
            SetShowRangeSpawners(_showRangeSpawners, false);
            _currentPointerSpawner.AllBuildAbleHighlightOff();
            _currentPointerSpawner = null;
        }
    }

    private void DragEnd()
    {
        if (!_currentPointerSpawner)
        {
            ExitBuildMode(true);
            return;
        }

        SetShowRangeSpawners(_showRangeSpawners, false);
        _currentPointerSpawner.AllBuildAbleHighlightOff();

        if (_currentPointerSpawner.IsEmpty && _currentPointerSpawner.CurrentOwner == OwnerType.Player)
        {
            if (_goldManager.EnoughGold(_buildingCard.Table.buildCost))
            {
                _buildingManager.SpawnBuilding(_buildingCard.Table.id, _currentPointerSpawner);
                _goldManager.UseGold(_buildingCard.Table.buildCost);
                ExitBuildMode();
            }
            else
            {
                ExitBuildMode(true);
                SystemUI.ShowNotEnoughGoldToastMessage();
            }
        }
        else
        {
            ExitBuildMode(true);
            _currentPointerSpawner = null;
        }
    }

    private void HandleTouch()
    {
        if (IsPointerOverUI())
            return;
        Vector3 worldPosition = GetWorldPosition(Input.mousePosition);
        var spawner = GetSpawnerAtPosition(worldPosition);
        SetShowRangeSpawners(_showRangeSpawners, false);

        if (spawner && !spawner.IsEmpty && spawner.CurrentOwner == OwnerType.Player)
        {
            SetShowRangeSpawners(_spawnerGridManager.GetNeighbors(spawner, spawner.Building.Table.showRange), true);
            spawner.Building.OpenInfo();
        }
    }

    private bool IsPointerOverUI()
    {
        var pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }

    private void SetShowRangeSpawners(List<Spawner> spawners, bool show)
    {
        if (_currentPointerSpawner)
        {
            spawners.Add(_currentPointerSpawner);
        }

        foreach (var s in spawners)
            s.ShowRangeHighlight(show);
        if (!show)
        {
            spawners.Clear();
        }
        else
        {
            _showRangeSpawners = spawners;
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        var pos = _camera.ScreenToWorldPoint(screenPosition);
        pos.z = 0f;
        return pos;
    }

    private Spawner GetSpawnerAtPosition(Vector3 worldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _spawnerLayerMask);
        return hit.collider ? hit.collider.GetComponentInParent<Spawner>() : null;
    }
}