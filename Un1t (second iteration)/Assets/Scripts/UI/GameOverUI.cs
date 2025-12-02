using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SpawnersManager;

public class GameOverUI : MonoBehaviour
{
    //[SerializeField] private EventParent parent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text gameoverText;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Button reload;
    [SerializeField] private Button quit;
    private MainUI mainUI;
    private PauseManager pauseManager;
    private PlayerModel playerModel;

    private void Awake()
    {
        canvas.worldCamera = Camera.current;
        reload.onClick.AddListener(ReloadScene);
        quit.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        pauseManager = mainUI.PauseManager;
        playerModel = mainUI.PlayerModelMB.PlayerModel;
        // TODO: move subscription in OnEnable after model initialization rework
        playerModel.PlayerDeath += OnPlayerDeath;
        //parent.LevelEnded.AddListener(OnLevelEnd); // <---- subscribe method on event
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

    private void OnLevelEnd()
    {
        canvas.enabled = true;
        gameoverText.text = winText;
    }

    private void ReloadScene()
    {
        pauseManager.ReloadScene();
    }

    private void QuitGame()
    {
        pauseManager.QuitGame();
    }
}