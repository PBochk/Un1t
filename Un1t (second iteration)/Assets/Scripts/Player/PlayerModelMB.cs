using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour, IActor
{
    [SerializeField] private PlayerConfig playerConfig;
    public PlayerModel PlayerModel { get; private set; }
    private const float REGEN_COOLDOWN = 5f;
    private bool isRegenReady = false;
    private void Awake()
    {
        PlayerModel = new(playerConfig.BaseMaxHealth, 
                          playerConfig.Level, 
                          playerConfig.XPToNextLevel,
                          playerConfig.BaseHealCostCoefficient,
                          playerConfig.BaseXPGainCoefficient,
                          playerConfig.BaseMovingSpeed, 
                          playerConfig.BaseDashSpeed,
                          playerConfig.BaseDashDuration,
                          playerConfig.BaseDashCooldown
                          );
        isRegenReady = true;
    }

    public void Initialize(IInstanceModel model)
    {
        PlayerModel = model as PlayerModel;
    }

    private void Update()
    {
        if (!isRegenReady) return;
        PlayerModel.Regenerate();
        StartCoroutine(WaitForRegeration());
    }

    private IEnumerator WaitForRegeration()
    {
        isRegenReady = false;
        yield return new WaitForSeconds(REGEN_COOLDOWN);
        isRegenReady = true;
    }
}
