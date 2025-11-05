using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HintObject : MonoBehaviour
{
    [SerializeField] private string hintText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HintManager.Instance.OpenHintWithText(hintText);
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    HintManager.Instance.CloseHint();
    //}
}
