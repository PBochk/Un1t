using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    //private MainUI mainUI;
    private PlayerRangeWeaponModel rangeModel;

    // TODO: move subscription in OnEnable after model initialization rework
    //private void Start()
    //{
    //    mainUI = GetComponentInParent<MainUI>();
    //    rangeModel = mainUI.PlayerModel.RangeModel;
    //    Initialize();
    //    rangeModel.AmmoChanged += OnAmmoChanged;
    //}
    //private void OnEnable()
    //{
    //    rangeModel.AmmoChanged += OnAmmoChanged;
    //}

    //private void OnDisable()
    //{
    //    rangeModel.AmmoChanged -= OnAmmoChanged;
    //}

    public void BindEvents(PlayerRangeWeaponModel rangeModel)
    {
        ammoText.text = rangeModel.Ammo.ToString();
        rangeModel.AmmoChanged += OnAmmoChanged;
    }

    //private void Initialize()
    //{
    //    OnAmmoChanged();
    //}

    private void OnAmmoChanged(int ammo)
    {
        ammoText.text = ammo.ToString();
    }
}