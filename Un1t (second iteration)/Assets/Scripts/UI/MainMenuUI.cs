using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] private int firstLevelSceneIndex;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private CanvasGroup buttons;
    [SerializeField] private Button newGame;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quit;
    [SerializeField] private Button openOptions;
    [SerializeField] private Button closeOptions;

    private void Awake()
    {
        newGame.onClick.AddListener(EntryPoint.Instance.LoadNewGame);
        continueButton.interactable = EntryPoint.Instance.IsLoadAvailable();
        continueButton.onClick.AddListener(() => EntryPoint.Instance.Load());
        openOptions.onClick.AddListener(OnOptionsOpened);
        quit.onClick.AddListener(Application.Quit);
        closeOptions.onClick.AddListener(OnOptionsClosed);
    }

    private void OnOptionsOpened()
    {
        buttons.interactable = false;
        optionsCanvas.enabled = true;
    }

    private void OnOptionsClosed()
    {
        optionsCanvas.enabled = false;
        buttons.interactable = true;
    }
}
