using UnityEngine;

[CreateAssetMenu(fileName = "RocksSpriites", menuName = "Scriptable Objects/RocksSpriites")]
public class RocksSprites : ScriptableObject
{
    public Sprite GetRandomSprite() => rocksSprites[Random.Range(0, rocksSprites.Length)];

    [SerializeField] private Sprite[] rocksSprites;
}
