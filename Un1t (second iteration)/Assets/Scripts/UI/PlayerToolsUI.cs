using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    private PlayerRangeWeaponModel rangeModel;

    // TODO: move subscription in OnEnable after model initialization rework
    private void Start()
    {
        var playerUI = GetComponentInParent<PlayerUI>();
        rangeModel = playerUI.PlayerRangeWeaponModelMB.RangeWeaponModel;
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
