using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayMoveSound()
    {
        source.PlayOneShot(moveSound);
    }
    
    public void PlayAttackSound()
    {
        source.PlayOneShot(attackSound);
    }

    public void PlaydDeathSound()
    {
        source.PlayOneShot(deathSound);
    }
}
