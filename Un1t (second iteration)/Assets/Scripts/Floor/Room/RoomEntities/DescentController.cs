using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DescentController : MonoBehaviour
{
    public UnityEvent OnPlayerEntered;
    [SerializeField] private int nextSceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerController>(out _)) return;
        //FindFirstObjectByType<EventParent>().NotifyLevelEnded();

        //OnPlayerEntered?.Invoke();
        EntryPoint.Instance.Save();
        EntryPoint.Instance.Load(3);
    }
}
