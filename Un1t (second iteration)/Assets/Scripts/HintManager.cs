using UnityEngine;
using UnityEngine.Events;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance;
    public UnityEvent<string> HintOpened;
    public UnityEvent HintClosed;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenHintWithText(string hintText)
    {
        HintOpened?.Invoke(hintText);
    }
    public void CloseHint() => HintClosed?.Invoke();
}
