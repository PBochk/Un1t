using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerUpgradeController upgradeController;
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PlayerMeleeWeaponModelMB playerMeleeModelMB;
    [SerializeField] private PlayerRangeWeaponModelMB playerRangeModelMB;
    
    public PlayerController PlayerController => playerController;
    public PauseManager PauseManager => pauseManager;
    public PlayerUpgradeController UpgradeController => upgradeController;
    public PlayerModelMB PlayerModelMB => playerModelMB;
    public PlayerMeleeWeaponModelMB PlayerMeleeModelMB => playerMeleeModelMB;
    public PlayerRangeWeaponModelMB PlayerRangeModelMB => playerRangeModelMB;

}
