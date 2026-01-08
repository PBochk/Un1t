using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] private int firstLevelSceneIndex;
    [SerializeField] private Button newGame;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button options;
    [SerializeField] private Button quit;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private CanvasGroup buttons;
    [SerializeField] private Button closeOptions;

    private void Awake()
    {
        newGame.onClick.AddListener(() => StartCoroutine(OnNewGameStart()));
        options.onClick.AddListener(OnOptionsOpened);
        quit.onClick.AddListener(Application.Quit);
        closeOptions.onClick.AddListener(OnOptionsClosed);
    }

    private IEnumerator OnNewGameStart()
    {
        var firstLevel = SceneManager.LoadSceneAsync(firstLevelSceneIndex);
        yield return new WaitWhile(() => !firstLevel.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstLevelSceneIndex));
        SceneManager.UnloadSceneAsync(mainMenuSceneIndex);
    }
    private void OnOptionsOpened()
    {
        buttons.interactable = false;
        optionsCanvas.gameObject.SetActive(true);
        Debug.Log(optionsCanvas.enabled);
    }
    private void OnOptionsClosed()
    {
        buttons.interactable = true;
        optionsCanvas.gameObject.SetActive(false);
    }
}
