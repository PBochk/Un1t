using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Button returnToMenu;
    private MainUI mainUI;

    private void Awake()
    {
        returnToMenu.onClick.AddListener(OnReturnToMenu);
        optionsCanvas.enabled = false;
    }

    private void Start()
    {
        mainUI = GetComponentInParent<MainUI>();
        returnToMenu.onClick.AddListener(mainUI.UIAudio.PlayButtonClickSound);
    }

    private void OnReturnToMenu()
    {
        optionsCanvas.enabled = false;
        menuCanvas.enabled = true;
    }
}