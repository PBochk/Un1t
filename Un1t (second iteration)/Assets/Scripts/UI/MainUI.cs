using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private UIAudio uiAudio;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameOverUI gameOverUI;
    private PlayerModel playerModel;

    public UIAudio UIAudio => uiAudio;
    public PlayerModel PlayerModel => playerModel;

    public void Initialize(PlayerModel playerModel)
    {
        this.playerModel = playerModel;
        var statsInstance = Instantiate(playerUI);
        statsInstance.BindEvents(playerModel);
        var gameOverInstance = Instantiate(gameOverUI);
        gameOverInstance.BindEvents(uiAudio, playerModel);
    }
}
