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
    [SerializeField] private RectTransform popupPoinnt;
    private BuildingInfoPopup _buildingInfoPopup;

    public void Set(BuildingCard buildingCard)
    {
        icon.sprite = ImageContainer.GetImage(buildingCard.Table.iconPath);
        BuildingInfoPopup.Args args = new BuildingInfoPopup.Args
        {
            Position = icon.transform.position,
            OpenType = BuildingPopupOpenType.DraggingBuilding,
            Building = null,
            Table = buildingCard.Table,
        };
        _buildingInfoPopup = _stageUI.OpenPopup(StageUI.PopupConfig.BuildingInfoPopupConfig, args).GetSlice<BuildingInfoPopup>();
    }

    public void Off()
    {
        gameObject.SetActive(false);
        _buildingInfoPopup.CloseView();
    }

    public Vector2 OnDrag(Vector2 screenPosition)
    {
        transform.position = screenPosition;
        _buildingInfoPopup.SetPosition(popupPoinnt.transform.position);
        return icon.transform.position;
    }
}