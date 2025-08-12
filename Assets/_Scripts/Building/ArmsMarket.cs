using UnityEngine;

public class ArmsMarket : Building
{
    private int GoldAmount => (int)Table.effectValue;

    protected override void OnCollisionPerformAction(IChanger changer)
    {
        PerformAction(new ActionContext(changer, GoldAmount));
    }
}