using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Interactions with scene for pause buttons
/// </summary>
// Below lies temporary solution for demonstration
// Actually is used by game over too
// TODO: implement better solution for pause buttons
// TODO: make a better name for class
public class PauseManager : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    private PlayerModel playerModel;
    private bool isPaused;
    public bool IsPaused => isPaused;

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        PauseScene(); // It is called beacause of hints
                      // TODO: remove this call
    }

    public void PauseScene()
    {
        Time.timeScale = 0f;
        playerModel.SetPlayerRestrained(true);
        isPaused = true;
    }

    public void UnpauseScene()
    {
        Time.timeScale = 1f;
        playerModel.SetPlayerRestrained(false);
        isPaused = false;
    }

    public void ReloadScene()
    {
        UnpauseScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
