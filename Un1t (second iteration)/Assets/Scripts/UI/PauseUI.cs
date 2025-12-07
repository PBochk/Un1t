using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button unpause;
    [SerializeField] private Button reload;
    [SerializeField] private Button quit;
    private MainUI mainUI;
    private PauseManager pauseManager;

    private void Awake()
    {
        canvas.worldCamera = Camera.current;
        unpause.onClick.AddListener(UnpauseScene);
        reload.onClick.AddListener(ReloadScene);
    }

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        pauseManager = mainUI.PauseManager;
        quit.onClick.AddListener(pauseManager.QuitGame);
        unpause.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        quit.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        reload.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
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
