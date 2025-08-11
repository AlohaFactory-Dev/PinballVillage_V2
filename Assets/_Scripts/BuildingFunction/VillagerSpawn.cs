using System.Collections.Generic;
using Aloha.Coconut;
using Stage.Building;
using UnityEngine;
using Zenject;

public class VillagerSpawn : BuildingFunction
{
    [Inject] VillagerManager _villagerManager;
    [SerializeField] VillagerType villagerType = VillagerType.Normal;
    List<Villager> _villagers = new List<Villager>();

    public override void PerformAction(IChanger changer, int value, Building.CalculateType calculate)
    {
        int count = _villagers.Count;
        if (calculate == Building.CalculateType.Add)
        {
            count = (int)Table.effectValue + value;
        }
        else if (calculate == Building.CalculateType.Multiply)
        {
            count = Mathf.CeilToInt(Table.effectValue * value);
        }

        for (int i = 0; i < count; i++)
        {
            _villagers.Add(_villagerManager.SpawnVillager(Spawner, villagerType));
        }

        if (Spawner.CurrentOwner == OwnerType.Player)
        {
            var floatingText = FactoryManager.FloatingTextFactory.GetText();
            var content = TextTableV2.Get("FloatingText/VillagerSpawn", new TextTableV2.Param("villagerType", $"Villager{villagerType}"), new TextTableV2.Param("value", $"{count}"));
            floatingText.SetText(content);
            floatingText.Play(Spawner.transform.position);
        }
    }

    public override void UpgradePerformance(float value)
    {
        _villagers.Add(_villagerManager.SpawnVillager(Spawner, villagerType));
    }

    public override void DestroyAction()
    {
        foreach (var villager in _villagers)
        {
            _villagerManager.RemoveVillager(villager);
        }

        _villagers.Clear();
    }
}