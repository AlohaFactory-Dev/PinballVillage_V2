using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DraggingBuildingPool
{
    private StageUI _stageUI;
    private readonly Stack<DraggingBuilding> _pool = new Stack<DraggingBuilding>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    public void Init()
    {
        _stageUI = StageContainer.Get<StageUI>();
    }

    public DraggingBuilding Get()
    {
        DraggingBuilding obj;
        if (_pool.Count > 0)
        {
            obj = _pool.Pop();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = StageContainer.Container.InstantiatePrefab(prefab, _stageUI.transform).GetComponent<DraggingBuilding>();
        }

        return obj;
    }

    public void Release(DraggingBuilding obj)
    {
        obj.Off();
        _pool.Push(obj);
    }
}