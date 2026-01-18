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
    private PlayerUpgradeController upgradeController;

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        upgradeController = PlayerUpgradeController.Instance;
        upgradeController.UpgradesChoiceSet.AddListener(OnLevelUp);
        canvas.gameObject.SetActive(false);
    }

    private void OnLevelUp(List<PlayerUpgrade> upgrades)
    {
        canvas.gameObject.SetActive(true);
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
    }
}
