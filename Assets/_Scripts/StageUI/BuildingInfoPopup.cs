using System;
using Aloha.Coconut;
using Aloha.Coconut.UI;
using Aloha.CoconutMilk;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum BuildingPopupOpenType
{
    DraggingBuilding,
    Building,
}

public class BuildingInfoPopup : UISlice, IDimClosable
{
    public class Args : UIOpenArgs
    {
        public Vector2 Position { get; set; }
        public BuildingPopupOpenType OpenType { get; set; }
        public Building Building { get; set; }
        public BuildingTable Table { get; set; }
    }

    [Inject] private BuildingManager _buildingManager;
    [Inject] private GoldManager _goldManager;

    [SerializeField] GameObject popupContent;
    [SerializeField] Button levelUpButton;
    [SerializeField] Button rotateButton;
    [SerializeField] Image buildingIcon;
    [SerializeField] Button restoreButton;
    [SerializeField] TextMeshProUGUI buildingNameText;
    [SerializeField] TextMeshProUGUI buildingDescriptionText;

    [SerializeField] TextMeshProUGUI buildingLevelText;

    private TextMeshProUGUI _levelUpCostText;
    private TextMeshProUGUI _restoreCostText;

    private Args _openArgs;

    private bool _isInitialized;
    private Camera _camera;
    private IDisposable _restoreSubscription;

    protected override void Open(UIOpenArgs args)
    {
        _openArgs = args as Args;
        Init();
        base.Open(_openArgs);

        if (_openArgs.Building)
        {
            _openArgs.Table = _openArgs.Building.Table;
        }

        buildingIcon.sprite = ImageContainer.GetImage(_openArgs.Table.iconPath);
        buildingNameText.text = TextTableV2.Get(_openArgs.Table.nameKey);
        buildingDescriptionText.text = TextTableV2.Get(_openArgs.Table.descriptionKey);


        bool isMaxLevel = TableManager.IsMagicNumber(_openArgs.Table.levelUpCost);
        if (isMaxLevel)
        {
            buildingLevelText.text = "Max";
        }
        else
        {
            buildingLevelText.text = $"Lv.{_openArgs.Table.level}";
        }

        levelUpButton.gameObject.SetActive(!isMaxLevel);

        if (_restoreSubscription != null)
        {
            _restoreSubscription.Dispose();
        }

        if (_openArgs.OpenType == BuildingPopupOpenType.Building)
        {
            if (_openArgs.Building.IsDestroyedReadOnly.Value)
            {
                _restoreSubscription = _openArgs.Building.IsDestroyedReadOnly
                    .Subscribe(isDestroyed =>
                    {
                        restoreButton.gameObject.SetActive(isDestroyed);
                        if (!isDestroyed)
                        {
                            levelUpButton.gameObject.SetActive(!_openArgs.Building.IsMaxLevel);
                            rotateButton.gameObject.SetActive(_openArgs.Table.group == BuildingGroupType.DirectionSign && OwnerType.Player == _openArgs.Building.Spawner.CurrentOwner);
                            _levelUpCostText.color = _goldManager.EnoughGold(_openArgs.Table.levelUpCost) ? Color.white : Color.red;
                            _restoreCostText.color = _goldManager.EnoughGold(_openArgs.Building.RestoreCost) ? Color.white : Color.red;
                            restoreButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            levelUpButton.gameObject.SetActive(false);
                            rotateButton.gameObject.SetActive(false);
                        }
                    }).AddTo(this);
                levelUpButton.gameObject.SetActive(false);
                rotateButton.gameObject.SetActive(false);
                restoreButton.gameObject.SetActive(true);
            }
            else
            {
                restoreButton.gameObject.SetActive(false);
                levelUpButton.gameObject.SetActive(!_openArgs.Building.IsMaxLevel);
                if (_openArgs.OpenType == BuildingPopupOpenType.DraggingBuilding)
                {
                    rotateButton.gameObject.SetActive(true);
                }
            }

            popupContent.transform.position = RectTransformUtility.WorldToScreenPoint(_camera, _openArgs.Position);
            rotateButton.gameObject.SetActive(_openArgs.Table.group == BuildingGroupType.DirectionSign && OwnerType.Player == _openArgs.Building.Spawner.CurrentOwner);
            _levelUpCostText.color = _goldManager.EnoughGold(_openArgs.Table.levelUpCost) ? Color.white : Color.red;
            _restoreCostText.color = _goldManager.EnoughGold(_openArgs.Building.RestoreCost) ? Color.white : Color.red;
            _restoreCostText.text = $"복구\n" +
                $"<sprite name=Gold>x{_openArgs.Building.RestoreCost}";
        }
        else if (_openArgs.OpenType == BuildingPopupOpenType.DraggingBuilding)
        {
            popupContent.transform.position = _openArgs.Position;
            levelUpButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            restoreButton.gameObject.SetActive(false);
        }

        _levelUpCostText.text = $"LevelUp:\n" +
            $"<sprite name=Gold>x{_openArgs.Table.levelUpCost}";
    }


    private void Init()
    {
        //Test
        if (_isInitialized) return;
        _isInitialized = true;
        _camera = Camera.main;
        _levelUpCostText = levelUpButton.GetComponentInChildren<TextMeshProUGUI>(true);
        _restoreCostText = restoreButton.GetComponentInChildren<TextMeshProUGUI>(true);
        levelUpButton.onClick.AddListener(OnLevelUpButtonClicked);
        rotateButton.onClick.AddListener(OnRotateButtonClicked);
        restoreButton.onClick.AddListener(OnRestoreBuildingClicked);
        _goldManager.GoldAmount.Subscribe(amount =>
        {
            if (_openArgs.OpenType == BuildingPopupOpenType.Building)
            {
                _levelUpCostText.color = _goldManager.EnoughGold(_openArgs.Table.levelUpCost) ? Color.white : Color.red;
                _restoreCostText.color = _goldManager.EnoughGold(_openArgs.Building.RestoreCost) ? Color.white : Color.red;
            }
        }).AddTo(this);
    }

    private void OnRestoreBuildingClicked()
    {
        if (_goldManager.EnoughGold(_openArgs.Building.RestoreCost))
        {
            _openArgs.Building.Restore();
            _goldManager.UseGold(_openArgs.Building.RestoreCost);
        }
    }

    private void OnLevelUpButtonClicked()
    {
        if (_goldManager.EnoughGold(_openArgs.Table.levelUpCost))
        {
            var levelupCost = _openArgs.Table.levelUpCost;
            var newBuilding = _buildingManager.LevelUpBuilding(_openArgs.Building.Spawner);
            _openArgs.Building = newBuilding;
            Open(_openArgs);
            levelUpButton.gameObject.SetActive(!_openArgs.Building.IsMaxLevel);
            _goldManager.UseGold(levelupCost);
        }
        else
        {
            SystemUI.ShowNotEnoughGoldToastMessage();
        }
    }

    public void SetPosition(Vector2 position)
    {
        popupContent.transform.position = position;
    }

    private void OnRotateButtonClicked()
    {
        (_openArgs.Building as DirectionSign).ChangeDirectionSelf();
    }

    public void CloseByDim()
    {
        CloseView();
    }
}