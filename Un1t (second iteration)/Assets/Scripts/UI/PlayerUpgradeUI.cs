using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class PlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeController upgradeManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_Text[] buttonTexts;
    private PlayerModel playerModel;

    private void Start()
    {
        var playerUI = GetComponentInParent<PlayerUI>();
        playerModel = playerUI.PlayerModelMB.PlayerModel;
        canvas.gameObject.SetActive(false);
        upgradeManager.UpgradesChoiceSet.AddListener(OnLevelUp);
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
        playerModel.SetPlayerRestrained(true);
        for (var i = 0; i < 3; i++)
        {
            BindButton(buttons[i], upgrades[i]);
            buttonTexts[i].text = upgrades[i].Description;
        }
    }

    private void BindButton(Button button, PlayerUpgrade upgrade)
    {
        button.onClick.AddListener(() =>
        {
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
        playerModel.SetPlayerRestrained(false);
    }
}
