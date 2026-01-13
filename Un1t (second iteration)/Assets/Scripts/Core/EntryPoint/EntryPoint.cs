using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Monobehaviour class, that creates gameplay snene (Main scene)
/// and managing initialization flow
/// game can be loaded normally (first default load on Awake) 
/// reloaded (default load, caused by OnReload)
/// saved and loaded from save file
/// </summary>
public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private int menuSceneIndex = 1;
    [SerializeField] private int firstLevelSceneIndex = 2;
    private int sceneIndex;
    private int lastSceneIndex;
    private GameState gameState;
    /// <summary>
    /// Passed to the SaveLoader, to support different types of serialization
    /// (Useful, for example, when you change serialized type from xml/json to binary on release)
    /// </summary>
    private GameSerializer serializer;
    private GameSaveLoader saveLoader = new();
    public static EntryPoint Instance;

    public void Awake()
    {
        lastSceneIndex = 0;
        sceneIndex = menuSceneIndex;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //You can't reference the Application not in method
        serializer = new XMLGameSerializer(Application.persistentDataPath + "/save.xml");
        LoadMenu();
    }

    public void LoadMenu()
    {
        lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneIndex = menuSceneIndex;
        StartCoroutine(LoadScene(isMenuLoad: true));
    }

    public void LoadNewGame()
    {
        lastSceneIndex = sceneIndex;
        sceneIndex = firstLevelSceneIndex;
        InitializeState();
        StartCoroutine(LoadScene());
    }

    public void Save(int nextSceneIndex = -1)
    {
        if (nextSceneIndex != -1)
        {
            sceneIndex = nextSceneIndex;
        }
        gameState.OnSave(nextSceneIndex);
        saveLoader.SaveGame(serializer, gameState);
    }

    /// <summary>
    /// loads from outside classes the game (using Restore state load)
    /// </summary>
    public void Load(int newSceneIndex = -1)
    {
        lastSceneIndex = sceneIndex;
        if (!saveLoader.TryLoadGame(serializer, ref gameState))
        {
            Debug.Log("Failed to load game");
            return;
        }
        if (newSceneIndex == -1)
        {
            sceneIndex = gameState.NextSceneIndex;
            if (gameState.NextSceneIndex == -1) throw new Exception("There is no save!");
            RestoreStateFromSave();
            StartCoroutine(LoadScene());
        }
        else
        {
            sceneIndex = newSceneIndex;
            StartCoroutine(LoadScene());
        }
    }


    /// <summary>
    /// A general method that loads the gameplay *scene*, using SceneManager
    /// </summary>
    private IEnumerator LoadScene(bool isMenuLoad = false)
    {
        var load = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!load.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        StartCoroutine(UnloadScene());
        if(!isMenuLoad) InitializeScene();
        BindEvents();
    }

    private void InitializeState()
    {
        gameState = new GameState(new PlayerModel(playerConfig));
    }

    private void InitializeScene()
    {
        gameState.PlayerModel.CreateInstance();
        Save();
    }

    private void RestoreStateFromSave()
    {
        gameState.OnLoadFromSave();
    }

    /// <summary>
    /// SceneManager.UnloadScene() is deprecated, and you can only use Async version, but since
    /// PlayerInput with UnityEvents invocation doesn't support Async methods, you simply create a coroutine
    /// that checks every frame if AsyncOperation is done yet
    /// There's also nothing wrong (i think) with creating and destroying scene at the same time
    /// </summary>
    private IEnumerator UnloadScene()
    {
        var unload = SceneManager.UnloadSceneAsync(lastSceneIndex);
        while (!unload.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Classes that require events from model data subscribe to it by itself
    /// </summary>
    private void BindEvents()
    {
        //UI.Instance.BindEvents(gameState);
    }

    public bool IsContinueAvailable()
    {
        var testState = new GameState();
        if (!saveLoader.TryLoadGame(serializer, ref testState)) return false;
        return testState.NextSceneIndex > 0 && testState.NextSceneIndex != menuSceneIndex;
    }

    public void OnPlayerDeath()
    {
        gameState.OnSaveDelete();
        Save(menuSceneIndex);
        LoadMenu();
    }
}