using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Sprite bareHands;
    [SerializeField] private Sprite melee;
    [SerializeField] private Sprite pickaxe;
    [SerializeField] private Sprite range;

    private Dictionary<PlayerTools, Sprite> toolImages;
    public Image currentTool;
    public TMP_Text AmmoText => ammoText;
    public Dictionary<PlayerTools, Sprite> ToolImages => toolImages;

    [SerializeField] private PlayerModelMB playerModelMB;
    private PlayerModel playerModel;

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

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        playerModel.ToolChanged += ChangeCurrentToolIcon;
    }

    private void OnDisable()
    {
        playerModel.ToolChanged -= ChangeCurrentToolIcon;
    }

    public void ChangeCurrentToolIcon(PlayerTools tool)
    {
        currentTool.sprite = toolImages[tool];
    }
}
