using UnityEngine;

public class ProjectileAudio : MonoBehaviour
{
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip wallHit;
    private AudioSource audioSource;
    private ProjectileController controller;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<ProjectileController>();
        controller.EnemyHit.AddListener(() => audioSource.PlayOneShot(enemyHit));
        controller.WallHit.AddListener(() => audioSource.PlayOneShot(wallHit));
    }

}