using UnityEngine;
using UnityEngine.Events;

public class DescentController : MonoBehaviour
{
    public UnityEvent OnPlayerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerController>(out _)) return;
        FindFirstObjectByType<EventParent>().NotifyLevelEnded();

        OnPlayerEntered?.Invoke();
    }
}
