using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private PlayerModelMB playerModelMB;
    private PlayerModel playerModel;

    // TODO: move subscription in OnEnable after model initialization rework
    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
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
    }
    private void OnExperienceChanged()
    {
        xpText.text = playerModel.CurrentXP + " / " + playerModel.NextLevelXP;
    }
}
