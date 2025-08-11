using System;
using Stage.Building;
using UnityEngine;

public class SpawnerOwnerChanger : MonoBehaviour
{
    public OwnerType CurrentOwner => ownerType;
    [SerializeField] OwnerType ownerType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Action<Villager> _onChange;
    [SerializeField] private BoxCollider2D boxCollider;
    public BoxCollider2D BoxCollider => boxCollider;

    public void Init(Action<Villager> onChange, Spawner spawner)
    {
        _onChange = onChange;
    }


    public bool EnableCheck(OwnerType ownerType)
    {
        bool isActive = this.ownerType == ownerType;
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }

        return isActive;
    }

    ///충돌이 벗어났을 때 처리
    ///이유: Enter를 쓰면 중복처리됨
    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.transform.TryGetComponent(out Villager villager) || !gameObject.activeSelf) return;
        _onChange(villager);
    }
#if UNITY_EDITOR
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
#endif
}