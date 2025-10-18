using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float movingSpeed;
    [SerializeField] private int level;
    [SerializeField] private int xpCoefficient;
    public PlayerModel playerModel;

    private void Awake()
    {
        // Temporary solution for serialization
        playerModel = new PlayerModel(maxHealth, movingSpeed, level, xpCoefficient);
    }
}
