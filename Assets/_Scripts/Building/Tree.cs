using Spine.Unity;
using UnityEngine;
using Zenject;

public class Tree : Building
{
    [SerializeField] private SpriteRenderer treeRenderer;

    [Header("Index 0 = 초기 나무 스프라이트")]
    [SerializeField]
    private string[] treeSkinNames;

    int _currentSpriteIndex = 0;
    [Inject] BuildingManager _buildingManager;
    [SerializeField] SkeletonMecanim skeletonMecanim;

    public override void Init(BuildingTable table, Spawner spawner, bool isLevelUp)
    {
        base.Init(table, spawner, isLevelUp);
        _currentSpriteIndex = 0;
        if (treeSkinNames.Length > 0)
        {
            ChangeSkin(treeSkinNames[_currentSpriteIndex]);
        }
    }


    protected override void OnCollisionPerformAction(IChanger changer)
    {
        if (_currentSpriteIndex >= treeSkinNames.Length) return; // 이미 모든 스프라이트를 사용한 경우


        PerformAction(new ActionContext(changer));
        _currentSpriteIndex++;
        if (_currentSpriteIndex < treeSkinNames.Length)
        {
            ChangeSkin(treeSkinNames[_currentSpriteIndex]);
        }
        else
        {
            _buildingManager.RemoveBuilding(this);
        }
    }

    private void ChangeSkin(string skinName)
    {
        skeletonMecanim.Skeleton.SetSkin(skinName);
        skeletonMecanim.Skeleton.SetSlotsToSetupPose();
        skeletonMecanim.LateUpdate(); // 즉시 반영하려면 호출
    }
}