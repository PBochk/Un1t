using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private AudioSource attackSound;
    public void Awake()
    {
        PlayerController controller = GetComponent<PlayerController>();
        controller.onMeleeAttack.AddListener(OnAttack);
    }

    private void OnAttack()
    {
        attackSound.Play();
    }

}
