using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Button unpause;
    [SerializeField] private Button reload;
    [SerializeField] private Button options;
    [SerializeField] private Button quit;
    private MainUI mainUI;

    private void Awake()
    {
        mainCanvas.worldCamera = Camera.current;
        unpause.onClick.AddListener(UnpauseScene);
        reload.onClick.AddListener(ReloadScene);
    }

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        options.onClick.AddListener(OnOptionsOpen);
        quit.onClick.AddListener(PauseManager.Instance.QuitGame);
        unpause.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        reload.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        options.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        quit.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
        mainCanvas.enabled = false;
    }

    private void OnPause()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            mainCanvas.enabled = true;
            PauseManager.Instance.PauseScene();
        }
    }

    private void UnpauseScene()
    {
        PauseManager.Instance.UnpauseScene();
        mainCanvas.enabled = false;
    }

    private void ReloadScene()
    {
        PauseManager.Instance.ReloadScene();
        mainCanvas.enabled = false;
    }

    private void OnOptionsOpen()
    {
        mainCanvas.enabled = false;
        optionsCanvas.enabled = true;
    }
}
