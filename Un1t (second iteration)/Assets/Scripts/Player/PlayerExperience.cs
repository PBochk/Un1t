using UnityEngine;

[RequireComponent(typeof(PlayerModelMB))]
public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience Instance;
    private PlayerModel model;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        model = GetComponent<PlayerModelMB>().PlayerModel;
    }

    public void AddXP(int increment)
    {
        model.IncreaseXP(increment);
    }
}
