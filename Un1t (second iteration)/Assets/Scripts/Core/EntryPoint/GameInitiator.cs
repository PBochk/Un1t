using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Documentation
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

    private void BindObjects()
    {
        mainCamera = Instantiate(mainCamera);
        player = Instantiate(player);
        enemySpawner = Instantiate(enemySpawner);
    }

    private void Initialize()
    {
    }

    private void Create()
    {
    }

    private void Prepare()
    {
    }
}
