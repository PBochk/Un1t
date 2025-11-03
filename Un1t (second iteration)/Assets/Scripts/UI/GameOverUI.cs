using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PauseManager pauseManager;
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
    }

    private void ReloadScene()
    {
        pauseManager.ReloadScene();
    }
}