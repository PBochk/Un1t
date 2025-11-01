using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button unpause;
    [SerializeField] private Button reload;
    [SerializeField] private Button quit;

    private void Awake()
    {
        unpause.onClick.AddListener(UnpauseScene);
        reload.onClick.AddListener(ReloadScene);
        quit.onClick.AddListener(pauseManager.QuitGame);
    }

    private void Start()
    {
        canvas.enabled = false;
    }

    private void OnPause()
    {
        if (!pauseManager.IsPaused)
        {
            canvas.enabled = true;
            pauseManager.PauseScene();
        }
    }

    private void UnpauseScene()
    {
        pauseManager.UnpauseScene();
        canvas.enabled = false;
    }

    private void ReloadScene()
    {
        pauseManager.ReloadScene();
        canvas.enabled = false;
    }
}
