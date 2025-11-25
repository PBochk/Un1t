using System.Collections;
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
        StartCoroutine(WaitForSetRestrained(true));
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseScene()
    {
        StartCoroutine(WaitForSetRestrained(false));
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Set player restrained in the next frame after call
    /// </summary>
    /// <remarks>
    /// Waiting is needed to not trigger melee attack on button click, for example 
    /// </remarks>
    /// <param name="isRestrained"></param>
    private IEnumerator WaitForSetRestrained(bool isRestrained)
    {
        yield return null;
        playerModel.SetPlayerRestrained(isRestrained);
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
