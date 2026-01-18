using System;
using System.Collections;
using UnityEngine;

public class BossInitiator : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private EnemyController bossPrefab;
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private float timeToSpawn;
    private bool isInitiated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInitiated) return;
        isInitiated = true;
        var target = other.GetComponentInParent<EnemyTargetComponent>();
        if (target == null) return;
        StartCoroutine(WaitForSpawn(target));
    }

    private IEnumerator WaitForSpawn(EnemyTargetComponent target)
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(timeToSpawn);
        Instantiate(door, transform);
        var boss = Instantiate(bossPrefab, spawnPlace);
        boss.SetTarget(target);
    }
}