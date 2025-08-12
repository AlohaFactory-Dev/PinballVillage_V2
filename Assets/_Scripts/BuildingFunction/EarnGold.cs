using Aloha.Coconut;
using UnityEngine;
using Zenject;

public class EarnGold : BuildingFunction
{
    [Inject] GoldManager _goldManager;
    private float _upgradeValue;
    private readonly string _floatingTextKey = "FloatingText/EarnGold";

    public override void PerformAction(ActionContext actionContext)
    {
        if (actionContext.Changer == null)
        {
            EarnGoldAction(actionContext.Value);
            return;
        }

        if (actionContext.Changer.OwnerType != OwnerType.Player) return;
        EarnGoldAction(actionContext.Value);
    }

    private void EarnGoldAction(int value)
    {
        var gold = Mathf.CeilToInt(_upgradeValue + Table.effectValue + value);
        _goldManager.AddGold(gold);

        string content = TextTableV2.Get(_floatingTextKey, new TextTableV2.Param("value", $"{gold}"));
        var floatingText = FactoryManager.FloatingTextFactory.GetText();
        floatingText.SetText(content);
        floatingText.Play(Spawner.transform.position);
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