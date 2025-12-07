using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class PlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] icons;
    [SerializeField] private TMP_Text[] upgradeNames;
    [SerializeField] private TMP_Text[] upgradeDescriptions;

    private MainUI mainUI;
    private PlayerModel playerModel;
    private PlayerController playerController;
    private PlayerUpgradeController upgradeController;

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        playerModel = mainUI.PlayerModelMB.PlayerModel;
        playerController = mainUI.PlayerController;
        upgradeController = mainUI.UpgradeController;
        upgradeController.UpgradesChoiceSet.AddListener(OnLevelUp);
        canvas.gameObject.SetActive(false);
    }

    // Experience model intialize later than OnEnable, so can't make it work rn
    // TODO: move subscription in OnEnable after model initialization rework
    //private void OnEnable()
    //{
    //    playerModel.NextLevel += OnLevelUp;
    //}

    //private void OnDisable()
    //{
    //    playerModel.NextLevel -= OnLevelUp;
    //}

    private void OnLevelUp(List<PlayerUpgrade> upgrades)
    {
        Debug.Log("OnLevelUp");
        canvas.gameObject.SetActive(true);
        playerController.SetPlayerRestrained(true);
        for (var i = 0; i < 3; i++)
        {
            BindButton(buttons[i], upgrades[i]);
            icons[i].sprite = upgrades[i].Icon;
            upgradeNames[i].text = upgrades[i].Name;
            upgradeDescriptions[i].text = upgrades[i].Description;
        }
    }

    private void BindButton(Button button, PlayerUpgrade upgrade)
    {
        button.onClick.AddListener(() =>
        {
            mainUI.UIAudio.PlayButtonClickSound();
            upgrade.Apply();
            DeactivateCanvas();
        });

    }

    private void DeactivateCanvas()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        canvas.gameObject.SetActive(false);
        playerController.SetPlayerRestrained(false);
    }
}
