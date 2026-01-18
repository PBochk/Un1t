using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class JumpTelegraphVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private readonly Color InitialColor = Color.white;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        spriteRenderer.color = InitialColor;
    }

    public void UpdateFade(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}