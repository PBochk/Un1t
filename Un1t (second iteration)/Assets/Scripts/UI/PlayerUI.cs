using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private Image levelUpIcon;
    [SerializeField] private Animator levelUpIconAnimator;
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Sprite[] xpSprites;
    //private MainUI mainUI;
    private PlayerModel playerModel;
    private int hpTilesCount;
    private int xpTilesCount;
    // TODO: move subscription in OnEnable after model initialization rework

    private void Awake()
    {
        hpTilesCount = hpSprites.Length - 1;
        xpTilesCount = xpSprites.Length - 1;
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void BindEvents(PlayerModel playerModel)
    {
        this.playerModel = playerModel;
        playerModel.HealthChanged += OnHealthChanged; //Should be in OnEnable
        playerModel.ExperienceChanged += OnExperienceChanged; //Should be in OnEnable
        playerModel.RangeModel.AmmoChanged += OnAmmoChanged;
        Initialize();
    }

    private void OnDisable()
    {
        playerModel.HealthChanged -= OnHealthChanged;
        playerModel.ExperienceChanged -= OnExperienceChanged;
        playerModel.RangeModel.AmmoChanged -= OnAmmoChanged;
    }

    private void Initialize()
    {
        OnAmmoChanged(playerModel.RangeModel.Ammo);
        OnHealthChanged();
        OnExperienceChanged();
    }

    private void OnHealthChanged()
    {
        hpText.text = playerModel.CurrentHealth + " / " + playerModel.MaxHealth;
        var number = playerModel.CurrentHealth <= 0 ? hpTilesCount : Mathf.FloorToInt((1 - playerModel.CurrentHealth / playerModel.MaxHealth) * hpTilesCount);
        hpBar.sprite = hpSprites[number];
    }

    private void OnExperienceChanged()
    {
        levelUpIconAnimator.SetBool("IsLevelUpAvailable", playerModel.IsLevelUpAvailable);
        xpText.text = playerModel.CurrentXP + " / " + playerModel.NextLevelXP;
        var number = playerModel.CurrentXP >= playerModel.NextLevelXP ? 0 : Mathf.FloorToInt((1 - ((float)playerModel.CurrentXP / playerModel.NextLevelXP)) * xpTilesCount);
        xpBar.sprite = xpSprites[number];
    }

    private void OnAmmoChanged(int ammo)
    {
        ammoText.text = ammo.ToString();
    }
}
