using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button unpause;
    [SerializeField] private Button reload;
    [SerializeField] private Button quit;
    private PlayerModel playerModel;

    private void Awake()
    {
        unpause.onClick.AddListener(UnpauseScene);
        reload.onClick.AddListener(ReloadScene);
        quit.onClick.AddListener(QuitGame);
        canvas.enabled = false;
    }

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
    }

    //private void OnEnable()
    //{
    //    playerModel.PlayerDeath += OnPlayerDeath;
    //}

    private void OnPause()
    {
        canvas.enabled = true;
        PauseScene();
    }


    // Below lies temporary solution for demonstration
    // TODO: remove from here
    private void PauseScene()
    {
        Time.timeScale = 0f;
        playerModel.SetPlayerRestrained(true);
    }

    private void UnpauseScene()
    {
        Time.timeScale = 1f;
        playerModel.SetPlayerRestrained(false);
        canvas.enabled = false;
    }

    private void ReloadScene()
    {
        UnpauseScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
