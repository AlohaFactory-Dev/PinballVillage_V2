using Aloha.Coconut;
using FactorySystem;
using UnityEngine;
using Zenject;

public class BuildingFloatingTextPoint : MonoBehaviour
{
    [Inject] private FactoryManager _factoryManager;
    [SerializeField] private Transform damageTextPoint;

    [Header("코인,주민 생성 텍스트 위치")]
    [SerializeField]
    private Transform defaultTextPoint;

    private string _damageTextKey = "FloatingText/Damage";
    private string _goldTextKey = "FloatingText/EarnGold";
    private string _villagerSpawnTextKey = "FloatingText/VillagerSpawn";
    private string _value = "value";
    private string _villagerType = "villagerType";
    private string _defaultFloatingTextId = "FloatingText";
    private string _damageFloatingTextId = "DamageFloatingText";

    public void ShowDamageText(int damage)
    {
        var text = _factoryManager.FloatingTextFactory.GetText(_damageFloatingTextId);
        text.Init(defaultTextPoint.position);
        text.SetText(TextTableV2.Get(_damageTextKey, new TextTableV2.Param(_value, $"{damage}")));
        text.Play();
    }

    public void ShowGoldText(int gold)
    {
        var text = _factoryManager.FloatingTextFactory.GetText(_defaultFloatingTextId);
        text.Init(defaultTextPoint.position);
        text.SetText(TextTableV2.Get(_goldTextKey, new TextTableV2.Param(_value, $"{gold}")));
        text.Play();
    }

    public void ShowVillagerSpawnText(int count, VillagerType villagerType)
    {
        var text = _factoryManager.FloatingTextFactory.GetText(_defaultFloatingTextId);
        text.Init(defaultTextPoint.position);
        text.SetText(TextTableV2.Get(_villagerSpawnTextKey,
            new TextTableV2.Param(_villagerType, $"Villager{villagerType}"),
            new TextTableV2.Param(_value, $"{count}")));
        text.Play();
    }
}