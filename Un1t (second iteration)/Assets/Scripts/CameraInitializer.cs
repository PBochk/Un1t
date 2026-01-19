using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraInitializer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private void Awake()
    {
        StartCoroutine(WaitForInitialize());
    }

    private IEnumerator WaitForInitialize()
    {
        yield return new WaitUntil(() => PlayerTransformProvider.Instance != null && PlayerTransformProvider.Instance.GetPlayerTransform() != null);
        cinemachineCamera.Follow = PlayerTransformProvider.Instance.GetPlayerTransform();

    }
}