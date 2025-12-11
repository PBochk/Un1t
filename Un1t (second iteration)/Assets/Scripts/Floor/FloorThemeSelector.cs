using UnityEngine;

public class FloorThemeSelector : MonoBehaviour
{
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite darkSprite;

    private void ChooseTheme()
    {
        FloorTheme floorTheme = FloorThemeManager.CurrentTheme;

        GetComponent<SpriteRenderer>().sprite = 
            floorTheme == FloorTheme.Light ? lightSprite : darkSprite;
    }

    private void Start() => ChooseTheme();
}
