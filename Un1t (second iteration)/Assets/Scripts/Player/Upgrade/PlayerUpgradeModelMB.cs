using System.Collections.Generic;
using UnityEngine;

// Temporary solution
// TODO: rework with EntryPoint
[RequireComponent(typeof(PlayerModelMB))]
public class PlayerUpgradeModelMB : MonoBehaviour
{
    [SerializeField] private List<PlayerUpgradeTypes> availableUpgrades;
    [SerializeField] private List<PlayerAbilityTypes> availableAbilities;
    public PlayerUpgradeModel PlayerUpgradeModel { get; private set; }

    private void Awake()
    {
        PlayerUpgradeModel = new(availableUpgrades, availableAbilities);
    }
}