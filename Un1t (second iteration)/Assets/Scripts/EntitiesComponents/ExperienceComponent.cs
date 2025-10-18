using UnityEngine;

public class ExperienceComponent : MonoBehaviour
{
    [SerializeField] private int xp;
    public int XP => xp;

    /// <summary>
    /// Add xp when entity dies
    /// </summary>
    // Destroying components is inefficient, so we need to make some kind of pool
    // TODO: rework this when object pools are implemented
    private void OnDestroy()
    {
        PlayerExperience.Instance.AddXP(xp);
    }
}
