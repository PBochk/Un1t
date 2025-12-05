using UnityEngine;

public class DescentController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This functional is for demo only
        if (!collision.TryGetComponent(out PlayerController player)) return;
        GameObject.FindWithTag("LevelEnder").GetComponent<EventParent>().NotifyLevelEnded();
        Debug.Log("Level completed!!!");

    }
}
