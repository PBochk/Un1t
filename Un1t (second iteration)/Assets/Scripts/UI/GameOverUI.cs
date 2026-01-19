using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text gameoverText;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Button toMenu;
    [SerializeField] private Button quit;
    public static GameOverUI Instance;

    private void Awake()
    {
        Instance = this;
        toMenu.onClick.AddListener(ToMenu);
        quit.onClick.AddListener(QuitGame);
        canvas.enabled = false;
    }

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

    private IEnumerator WaitForDeathAnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        canvas.enabled = true;
        gameoverText.text = loseText;
    }

    public void OnBossDeath() => StartCoroutine(WaitForBossDeath());
    private IEnumerator WaitForBossDeath()
    {
        yield return new WaitForSeconds(2f);
        canvas.enabled = true;
        gameoverText.text = winText;
        PauseManager.Instance.PauseScene();
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