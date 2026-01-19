using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FloorThemeSelector : MonoBehaviour
{
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite darkSprite;
    [SerializeField] private Sprite glitchSprite;

    private void ChooseTheme()
    {
        FloorTheme floorTheme = FloorThemeManager.CurrentTheme;

        GetComponent<SpriteRenderer>().sprite = 
            floorTheme == FloorTheme.Light 
            ? lightSprite 
            : floorTheme == FloorTheme.Dark
            ? darkSprite
            : glitchSprite;
    }

    private void Start() => ChooseTheme();
}
