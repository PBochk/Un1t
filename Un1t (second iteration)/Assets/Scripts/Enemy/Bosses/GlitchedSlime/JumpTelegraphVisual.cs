using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class JumpTelegraphVisual : MonoBehaviour
{
    private const float ScaleFactor = .3f;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private Vector3 initialScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        initialScale = transform.localScale;
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
        var color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
        transform.localScale = initialScale;
    }

    public void UpdateFade(float alpha)
    {
        var color = spriteRenderer.color;
        transform.localScale = Vector3.Lerp(initialScale, initialScale * ScaleFactor, alpha);
        color.a = alpha;
        spriteRenderer.color = color;
    }
}