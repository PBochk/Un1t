using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text hint;
    private void Start()
    {
        HintManager.Instance.HintOpened?.AddListener(OnHintOpened);
        HintManager.Instance.HintClosed?.AddListener(OnHintClosed);
        OnHintClosed();
    }

    private void OnHintOpened(string hintText)
    {
        hint.text = hintText;
        canvas.gameObject.SetActive(true);
    }

    private void OnHintClosed()
    {
        canvas.gameObject.SetActive(false);
    }
}
