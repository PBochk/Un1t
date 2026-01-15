using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SpawnersManager;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text gameoverText;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Button toMenu;
    [SerializeField] private Button quit;

    private void Awake()
    {
        canvas.worldCamera = Camera.current;
        toMenu.onClick.AddListener(ToMenu);
        quit.onClick.AddListener(QuitGame);
        canvas.enabled = false;
    }

    private void Start()
    {
        // TODO: move subscription in OnEnable after model initialization rework
        canvas.enabled = false;

    }

    //private void OnEnable()
    //{
    //    playerModel.PlayerDeath += OnPlayerDeath;
    //}

    //private void OnDisable()
    //{
    //    playerModel.PlayerDeath -= OnPlayerDeath;
    //}

    public void BindEvents(UIAudio audio, PlayerModel playerModel)
    {
        playerModel.PlayerDeath += OnPlayerDeath;
        toMenu.onClick.AddListener(audio.PlayButtonClickSound);
        quit.onClick.AddListener(audio.PlayButtonClickSound);
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(WaitForDeathAnimationEnd());
    }


    // Temporary solution
    // TODO: make a better one
    private IEnumerator WaitForDeathAnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        canvas.enabled = true;
        gameoverText.text = loseText;
    }

    private void OnGameCompleted()
    {
        canvas.enabled = true;
        gameoverText.text = winText;
    }

    private void ToMenu()
    {
        EntryPoint.Instance.LoadMenu();
    }

    private void QuitGame()
    {
        PauseManager.Instance.QuitGame();
    }
}