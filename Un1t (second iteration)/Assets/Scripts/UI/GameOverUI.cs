using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    //[SerializeField] private EventParent parent; // <---- serialize object that has event LevelEnded
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text gameoverText;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Button reload;
    [SerializeField] private Button quit;
    private PlayerModel playerModel;

    private void Awake()
    {
        reload.onClick.AddListener(ReloadScene);
        quit.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
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