using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerUpgradeController upgradeController;
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PlayerMeleeWeaponModelMB playerMeleeModelMB;
    [SerializeField] private PlayerRangeWeaponModelMB playerRangeModelMB;
    [SerializeField] private UIAudio uiAudio;

    public PauseManager PauseManager => pauseManager;
    public AudioMixer AudioMixer => audioMixer;
    public PlayerController PlayerController => playerController;
    public PlayerUpgradeController UpgradeController => upgradeController;
    public PlayerModelMB PlayerModelMB => playerModelMB;
    public PlayerMeleeWeaponModelMB PlayerMeleeModelMB => playerMeleeModelMB;
    public PlayerRangeWeaponModelMB PlayerRangeModelMB => playerRangeModelMB;
    public UIAudio UIAudio => uiAudio;
}
