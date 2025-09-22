using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pretty simple solution for initialization of the game to happen
/// this is especially needed for convenient separate testing of new features
/// because git can't merge unity scene files, the initialization goes in 5 steps:
/// Binding, Initialization, Creation and Preparation
/// </summary>
public class GameInitiator : MonoBehaviour
{
    [SerializeField] private string MainScenePath;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera mainCamera;
    
    private void Awake()
    {
        SceneManager.LoadScene(MainScenePath, LoadSceneMode.Additive);
        BindObjects();
        Initialize();
        Create();
        Prepare();
    }

    /// <summary>
    /// To add something to the game, just link a prefab of it and create an instance of this
    /// only the base systems are created on this step
    /// </summary>
    private void BindObjects()
    {
        mainCamera = Instantiate(mainCamera);
        player = Instantiate(player);
        enemySpawner = Instantiate(enemySpawner);
    }

    /// <summary>
    /// On this step some additional initialization of previously instantiated systems is happening,
    /// for example, <see cref="EnemySpawner"/>'s target setting should be here
    /// </summary>
    private void Initialize()
    {
    }

    /// <summary>
    /// On this step, some heavy object creation and resource loading should happen,
    /// for example there could be enemy spawning for test scene
    /// </summary>
    private void Create()
    {
    }

    /// <summary>
    /// On this step, some final actions are performed, for example, updating ui, showing starting animation,
    /// hiding loading screen etc
    /// </summary>
    private void Prepare()
    {
    }
}
