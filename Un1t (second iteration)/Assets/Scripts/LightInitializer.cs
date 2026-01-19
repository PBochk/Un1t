using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
