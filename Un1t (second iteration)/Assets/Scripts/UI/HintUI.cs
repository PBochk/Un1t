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
    private MainUI mainUI;
    private PauseManager pauseManager;
    private int count = 0;
    private void Awake()
    {
        canvas.worldCamera = Camera.current;
        button.onClick.AddListener(NextHint);
    }
    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        pauseManager = mainUI.PauseManager;
        pauseManager.PauseScene();
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
