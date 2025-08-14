using System.Collections;
using System.Collections.Generic;
using Aloha.Coconut;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Selectable))]
public class BuildingCard : MonoBehaviour, IPointerDownHandler
{
    [Inject] private BuildingPoolManager _poolManager;
    [Inject] private BuildModeManager _buildModeManager;
    [Inject] private GoldManager _goldManager;
    [SerializeField] private GameObject[] gradeBgs;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI leveltext;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button infoButton;
    private Animator _animator;
    private BuildingTable _table;
    private Selectable _selectable;
    private int _refreshTriggerId;
    private int _idleTriggerId;
    private int _dragTriggerId;
    public BuildingTable Table => _table;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _selectable = GetComponent<Selectable>();
        _refreshTriggerId = Animator.StringToHash("Refresh");
        _idleTriggerId = Animator.StringToHash("Idle");
        _dragTriggerId = Animator.StringToHash("Drag");

        Draw();
        _goldManager.GoldAmount.Subscribe(amount =>
        {
            if (_table != null)
            {
                UpdateGoldText();
            }
        }).AddTo(this);
        UpdateGoldText();
    }

    public void Draw()
    {
        _table = _poolManager.Draw();

        for (int i = 0; i < gradeBgs.Length; i++)
        {
            gradeBgs[i].SetActive(false);
        }

        _animator.SetTrigger(_refreshTriggerId);
        icon.sprite = ImageContainer.GetImage(_table.iconPath);
        goldText.text = $"<sprite name=Gold>{_table.buildCost}";

        leveltext.text = "Lv." + _table.level;

        gradeBgs[_table.grade - 1].SetActive(true);
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        Color color = _goldManager.EnoughGold(_table.buildCost) ? Color.white : Color.red;
        goldText.color = color;
    }

    public void Refesh()
    {
        _table = _poolManager.RefreshDraw(_table);
        for (int i = 0; i < gradeBgs.Length; i++)
        {
            gradeBgs[i].SetActive(false);
        }

        _animator.SetTrigger(_refreshTriggerId);
        UpdateGoldText();
        icon.sprite = ImageContainer.GetImage(_table.iconPath);
        goldText.text = $"<sprite name=Gold>{_table.buildCost}";
        leveltext.text = "Lv." + _table.level;

        gradeBgs[_table.grade - 1].SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_selectable.interactable)
        {
            _animator.SetTrigger(_dragTriggerId);
            _buildModeManager.EnterBuildMode(this);
        }
    }

    public void DragEnd()
    {
        _animator.SetTrigger(_idleTriggerId);
    }
}