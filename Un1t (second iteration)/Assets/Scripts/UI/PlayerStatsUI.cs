using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Sprite[] xpSprites;
    private PlayerModel playerModel;
    private int hpCount;
    private int xpCount;
    // TODO: move subscription in OnEnable after model initialization rework

    private void Awake()
    {
        hpCount = hpSprites.Length - 1;
        xpCount = xpSprites.Length - 1;
    }

    private void Start()
    {
        playerModel = GetComponentInParent<PlayerUI>().PlayerModelMB.PlayerModel;
        playerModel.HealthChanged += OnHealthChanged; //Should be in OnEnable
        playerModel.ExperienceChanged += OnExperienceChanged; //Should be in OnEnable
        Initialize();
    }

    private void OnDisable()
    {
        playerModel.HealthChanged -= OnHealthChanged;
        playerModel.ExperienceChanged -= OnExperienceChanged;
    }

    private void Initialize()
    {
        OnHealthChanged();
        OnExperienceChanged();
    }

    private void OnHealthChanged()
    {
        hpText.text = playerModel.CurrentHealth + " / " + playerModel.MaxHealth;
        var number = playerModel.CurrentHealth <= 0 ? hpCount : Mathf.FloorToInt((1 - playerModel.CurrentHealth / playerModel.MaxHealth) * hpCount);
        hpBar.sprite = hpSprites[number];
    }
    private void OnExperienceChanged()
    {
        xpText.text = playerModel.CurrentXP + " / " + playerModel.NextLevelXP;
        var number = Mathf.FloorToInt((1 - ((float)playerModel.CurrentXP / playerModel.NextLevelXP)) * xpCount);
        xpBar.sprite = xpSprites[number];
    }
}
