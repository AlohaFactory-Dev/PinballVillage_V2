using Aloha.Coconut;
using UnityEngine;
using Zenject;

public class EarnGold : BuildingFunction
{
    [Inject] GoldManager _goldManager;
    private readonly string _floatingTextKey = "FloatingText/EarnGold";

    public override void PerformAction(ActionContext actionContext)
    {
        if (actionContext.Value <= 0)
        {
            return; // PerformAction이 호출되었지만, Value가 0인 경우는 무시
        }

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
        var gold = Mathf.CeilToInt(UpgradeValue + value);
        _goldManager.AddGold(gold);
        FloatingTextPoint.ShowGoldText(gold);
    }

    public override void UpgradePerformance(Passive passive)
    {
        UpgradeValue += passive.UpgradeValue;
    }

    public override void DowngradePerformance(Passive passive)
    {
        UpgradeValue -= passive.UpgradeValue;
    }

    public override void DestroyAction()
    {
        // Do nothing for now
    }
}