using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private PlayerExperienceModel experienceModel;
    public PlayerModel playerModel;

    private void Awake()
    {
        // Temporary solution for serialization
        playerModel = new PlayerModel(movingSpeed, healthComponent, experienceModel);
    }
}
