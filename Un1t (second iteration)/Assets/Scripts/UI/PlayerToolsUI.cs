using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    private MainUI mainUI;
    private PlayerRangeWeaponModel rangeModel;

    // TODO: move subscription in OnEnable after model initialization rework
    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        rangeModel = mainUI.PlayerRangeModelMB.RangeWeaponModel;
        rangeModel.AmmoChanged += OnAmmoChanged;
        Initialize();
    }

    private void Initialize()
    {
        OnAmmoChanged();
    }

    private void OnAmmoChanged()
    {
        ammoText.text = rangeModel.Ammo.ToString();
    }
}
