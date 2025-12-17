using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Button returnToMenu;

    private void Awake()
    {
        returnToMenu.onClick.AddListener(OnReturnToMenu);
        optionsCanvas.enabled = false;
    }

    private void OnReturnToMenu()
    {
        optionsCanvas.enabled = false;
        menuCanvas.enabled = true;
    }
}