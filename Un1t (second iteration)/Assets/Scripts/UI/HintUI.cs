using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text current;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private List<string> texts;
    private int count = 0;
    private void Awake()
    {
        button.onClick.AddListener(NextHint);
    }

    private void Start()
    {
        NextHint();
    }

    private void NextHint()
    {
        if (count >= texts.Count)
        {
            canvas.gameObject.SetActive(false);
            pauseManager.UnpauseScene();
        }
        else
        {
            current.text = texts[count];
            count++;
        }
    }
}
