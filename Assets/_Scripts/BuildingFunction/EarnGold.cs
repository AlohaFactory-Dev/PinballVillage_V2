using Aloha.Coconut;
using UnityEngine;
using Zenject;

public class EarnGold : BuildingFunction
{
    [Inject] GoldManager _goldManager;
    private float _upgradeValue;
    private string floatingTextKey = "FloatingText/EarnGold";

    public override void PerformAction(IChanger changer, int value, Building.CalculateType calculate)
    {
        if (changer == null)
        {
            var gold = Mathf.CeilToInt(_upgradeValue + Table.effectValue + value);
            _goldManager.AddGold(gold);

            string content = TextTableV2.Get(floatingTextKey, new TextTableV2.Param("value", gold.ToString()));
            var floatingText = FactoryManager.FloatingTextFactory.GetText();
            floatingText.SetText(content);
            floatingText.Play(Spawner.transform.position);
            return;
        }

        if (changer.OwnerType == OwnerType.Player)
        {
            if (value == 0)
            {
                value = 1; // Ensure at least one gold is earned
            }

            var gold = Mathf.CeilToInt((_upgradeValue + Table.effectValue) * value);
            _goldManager.AddGold(gold);

            string content = TextTableV2.Get(floatingTextKey, new TextTableV2.Param("value", gold.ToString()));
            var floatingText = FactoryManager.FloatingTextFactory.GetText();
            floatingText.SetText(content);
            floatingText.Play(Spawner.transform.position);
        }
    }


    public override void UpgradePerformance(float value)
    {
        _upgradeValue += value;
    }

    public override void DestroyAction()
    {
        // Do nothing for now
    }
}