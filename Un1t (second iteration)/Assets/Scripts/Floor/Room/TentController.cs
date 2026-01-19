using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TentController : MonoBehaviour
{
    [SerializeField] private Sprite[] outerViewSprites;
    [SerializeField] private Sprite innerViewSprite;

    [SerializeField] private BoxCollider2D[] wallsColliders;
    [SerializeField] private BoxCollider2D outerTentCollider;
    [SerializeField] private CapsuleCollider2D enteranceCollider;

    private SpriteRenderer spriteRenderer;
    private Sprite chosenSprite;

    private const string GROUND_DECORATIONS_LAYER = "Tent";
    private const string DARKNESS_LAYER = "Darkness";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        chosenSprite = outerViewSprites[Random.Range(0, outerViewSprites.Length)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (!collision.TryGetComponent<PlayerController>(out _)) return;
        spriteRenderer.sprite = innerViewSprite;
        spriteRenderer.sortingLayerName = GROUND_DECORATIONS_LAYER;
        outerTentCollider.isTrigger = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerController>(out _)) return;
        if (!collision.IsTouching(enteranceCollider) && !collision.IsTouching(outerTentCollider))
        {
            foreach (BoxCollider2D wall in wallsColliders)
                wall.enabled = false;
            outerTentCollider.isTrigger = false;

            spriteRenderer.sprite = chosenSprite;
            spriteRenderer.sortingLayerName = DARKNESS_LAYER;

            return;
        }

        foreach (BoxCollider2D wall in wallsColliders)
            wall.enabled = true;
    }
}
