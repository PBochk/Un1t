using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text current;
    [SerializeField] private List<string> texts;
    private int count = 0;
    private void Awake()
    {
        button.onClick.AddListener(NextHint);
        NextHint();
    }

    private void NextHint()
    {
        if (count >= texts.Count) canvas.gameObject.SetActive(false);
        else current.text = texts[count];
        count++;
    }
}
