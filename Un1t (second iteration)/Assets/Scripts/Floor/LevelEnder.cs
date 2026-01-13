using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex;
    public static LevelEnder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void EndLevel()
    {
        EntryPoint.Instance.Save();
        EntryPoint.Instance.Load(nextSceneIndex);
    }
}