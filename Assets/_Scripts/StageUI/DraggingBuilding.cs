using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class DraggingBuilding : MonoBehaviour
{
    [Inject] private StageUI _stageUI;
    [SerializeField] private Image icon;
    [SerializeField] private Transform pointer;
    [SerializeField] private RectTransform popupPoinnt;
    private BuildingInfoPopup _buildingInfoPopup;

    private Vector2 _initialIconLocalPosition;

    private bool _isInitialized = false;

    public void Set(BuildingCard buildingCard)
    {
        Init();
        icon.sprite = ImageContainer.GetImage(buildingCard.Table.iconPath);
        BuildingInfoPopup.Args args = new BuildingInfoPopup.Args
        {
            Position = popupPoinnt.position,
            OpenType = BuildingPopupOpenType.DraggingBuilding,
            Building = null,
            Table = buildingCard.Table,
        };
        _buildingInfoPopup = _stageUI.OpenPopup(StageUI.PopupConfig.BuildingInfoPopupConfig, args).GetSlice<BuildingInfoPopup>();
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _initialIconLocalPosition = icon.transform.localPosition;
    }

    public void Off()
    {
        gameObject.SetActive(false);
        _buildingInfoPopup.CloseView();
    }

    public void SnapToSpawner(Vector2 spawner)
    {
        icon.transform.position = spawner;
    }

    public void SnapToPointer()
    {
        icon.transform.localPosition = _initialIconLocalPosition;
    }

    public void SetPopupPosition()
    {
        _buildingInfoPopup.SetPosition(popupPoinnt.transform.position);
    }

    public Vector2 OnDrag(Vector2 screenPosition)
    {
        transform.position = screenPosition;
        return pointer.position;
    }
}