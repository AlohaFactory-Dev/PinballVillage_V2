using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;


public abstract class BuildingFunction : MonoBehaviour
{
    protected BuildingTable Table { get; set; }
    public abstract void PerformAction(ActionContext actionContext);


    public abstract void UpgradePerformance(Passive passive);
    public abstract void DowngradePerformance(Passive passive);
    protected Spawner Spawner;
    protected int UpgradeValue;
    protected BuildingFloatingTextPoint FloatingTextPoint;

    public void Init(BuildingTable buildingTable, Spawner spawner)
    {
        Table = buildingTable;
        Spawner = spawner;
        FloatingTextPoint = spawner.Building.FloatingTextPoint;
        UpgradeValue = 0;
    }

    public abstract void DestroyAction();
}