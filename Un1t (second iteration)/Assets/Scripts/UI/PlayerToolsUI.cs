using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolsUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PlayerRangeWeaponModelMB rangeModelMB;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Image currentTool;
    [SerializeField] private Sprite bareHands;
    [SerializeField] private Sprite melee;
    [SerializeField] private Sprite pickaxe;
    [SerializeField] private Sprite range;

    private PlayerModel playerModel;
    private PlayerRangeWeaponModel rangeModel;
    private Dictionary<PlayerTools, Sprite> toolImages;

    private void Awake()
    {
        toolImages = new Dictionary<PlayerTools, Sprite>
        {
            { PlayerTools.None, bareHands},
            { PlayerTools.Melee, melee},
            { PlayerTools.Pickaxe, pickaxe},
            { PlayerTools.Range, range}
        };
    }

    // TODO: move subscription in OnEnable after model initialization rework
    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        playerModel.ToolChanged += ChangeCurrentToolIcon;
        rangeModel = rangeModelMB.PlayerRangeWeaponModel;
        rangeModel.AmmoChanged += OnAmmoChanged;
        Initialize();
    }

    private void OnDisable()
    {
        playerModel.ToolChanged -= ChangeCurrentToolIcon;
    }

    private void Initialize()
    {
        OnAmmoChanged();
    }

    public void ChangeCurrentToolIcon(PlayerTools tool)
    {
        currentTool.sprite = toolImages[tool];
    }
    private void OnAmmoChanged()
    {
        ammoText.text = rangeModel.Ammo.ToString();
    }
}
