using System;
using System.Threading.Tasks;
using _Scripts;
using Aloha.Coconut.UI;
using FactorySystem;
using Stage.Building;
using UnityEngine;
using Zenject;

public class StageInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        StageContainer.Initialize(Container);

        var coconutCanvas = GetComponentInChildren<CoconutCanvas>();
        var stageGlobalClock = GetComponentInChildren<StageGlobalClock>();
        var spawnerGridManager = GetComponentInChildren<SpawnerGridManager>();
        var factoryManager = GetComponentInChildren<FactoryManager>();
        var stageManager = GetComponentInChildren<StageManager>();
        var buildModeManager = GetComponentInChildren<BuildModeManager>();
        var stageUI = GetComponentInChildren<StageUI>();
        var cameraController = GetComponentInChildren<CameraController>();


        Container.Bind<CoconutCanvas>().FromInstance(coconutCanvas).AsSingle().NonLazy();
        Container.Bind<StageGlobalClock>().FromInstance(stageGlobalClock).AsSingle().NonLazy();
        Container.Bind<SpawnerGridManager>().FromInstance(spawnerGridManager).AsSingle().NonLazy();
        Container.Bind<GoldManager>().AsSingle().NonLazy();
        Container.Bind<VillagerManager>().AsSingle().NonLazy();
        Container.Bind<BuildingManager>().AsSingle().NonLazy();


        Container.Bind<FactoryManager>().FromInstance(factoryManager).AsSingle().NonLazy();

        Container.Bind<BuildingPoolManager>().AsSingle().NonLazy();

        Container.Bind<StageManager>().FromInstance(stageManager).AsSingle().NonLazy();
        Container.Bind<StageUI>().FromInstance(stageUI).AsSingle().NonLazy();

        Container.Bind<BuildModeManager>().FromInstance(buildModeManager).AsSingle().NonLazy();
        Container.Bind<EnemyAutoBuildManager>().AsSingle().NonLazy();

        Container.Bind<CameraController>().FromInstance(cameraController).AsSingle().NonLazy();
        Init();
    }

    private async void Init()
    {
        await Container.Resolve<FactoryManager>().Init(Container);
        Container.Resolve<SpawnerGridManager>().Init();
        Container.Resolve<StageGlobalClock>().Init();
    }
}