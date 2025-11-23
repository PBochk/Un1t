using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour, IActor
{
    [SerializeField] private PlayerConfig playerConfig;
    public PlayerModel PlayerModel;

    private void Awake()
    {
        PlayerModel = new(playerConfig.BaseMaxHealth, 
                          playerConfig.BaseMovingSpeed, 
                          playerConfig.BaseDashSpeed,
                          playerConfig.BaseDashDuration,
                          playerConfig.BaseDashCooldown,
                          playerConfig.Level, 
                          playerConfig.XPToNextLevel);
    }

    public void Initialize(IInstanceModel model)
    {
        PlayerModel = model as PlayerModel;
    }
}
