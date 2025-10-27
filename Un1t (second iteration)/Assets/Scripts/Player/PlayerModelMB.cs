using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthUpgrade;
    [SerializeField] private float movingSpeed;
    [SerializeField] private int level;
    [SerializeField] private int xpCoefficient;
    public PlayerModel PlayerModel;

    private void Awake()
    {
        // Temporary solution for serialization
        PlayerModel = new PlayerModel(maxHealth, healthUpgrade, movingSpeed, level, xpCoefficient);
    }
}
