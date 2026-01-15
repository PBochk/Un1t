using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private UIAudio uiAudio;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameOverUI gameOverUI;
    private PlayerModel playerModel;
    private PlayerController playerController;

    public UIAudio UIAudio => uiAudio;
    public PlayerModel PlayerModel => playerModel;
    public PlayerController PlayerController => playerController;

    public void Initialize(PlayerModel playerModel, PlayerController playerController)
    {
        this.playerModel = playerModel;
        this.playerController = playerController;
        var statsInstance = Instantiate(playerUI);
        statsInstance.BindEvents(playerModel);
        var gameOverInstance = Instantiate(gameOverUI);
        gameOverInstance.BindEvents(uiAudio, playerModel);
    }
}
