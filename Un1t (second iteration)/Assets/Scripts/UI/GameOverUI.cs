using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button reload;
    private PlayerModel playerModel;

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        // TODO: move subscription in OnEnable after model initialization rework
        playerModel.PlayerDeath += OnPlayerDeath;
        reload.onClick.AddListener(ReloadScene);
        canvas.enabled = false;
    }

    //private void OnEnable()
    //{
    //    playerModel.PlayerDeath += OnPlayerDeath;
    //}

    private void OnDisable()
    {
        playerModel.PlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
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

    private void ReloadScene()
    {
        Time.timeScale = 1f;
        playerModel.SetPlayerRestrained(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
