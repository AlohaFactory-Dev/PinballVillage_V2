using Aloha.Coconut;
using UnityEngine;
using Zenject;

public class EarnGold : BuildingFunction
{
    [Inject] GoldManager _goldManager;
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
        var gold = Mathf.CeilToInt(UpgradeValue + Table.effectValue + value);
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