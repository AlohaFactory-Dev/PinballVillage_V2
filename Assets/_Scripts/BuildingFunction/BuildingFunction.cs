using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;


public abstract class BuildingFunction : MonoBehaviour
{
    protected BuildingTable Table { get; set; }
    [Inject] protected FactoryManager FactoryManager;


    public abstract void PerformAction(ActionContext actionContext);


    public abstract void UpgradePerformance(float value);
    protected Spawner Spawner;

    public void Init(BuildingTable buildingTable, Spawner spawner)
    {
        Table = buildingTable;
        Spawner = spawner;
    }

    public abstract void DestroyAction();
}