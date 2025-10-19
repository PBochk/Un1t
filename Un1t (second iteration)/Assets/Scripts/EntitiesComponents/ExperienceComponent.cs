using UnityEngine;

public class ExperienceComponent : MonoBehaviour
{
    [SerializeField] private int xp;

    /// <summary>
    /// Set xp if it isn't already assigned via serialization or this method
    /// </summary>
    public void SetXP(int xp)
    {
        if(this.xp == 0) 
            this.xp = xp;
    }

    /// <summary>
    /// Add xp to player. Should be subscribed in entity model on event like enemy death or end of world_event
    /// </summary>
    public void OnDropXP()
    {
        PlayerExperience.Instance.AddXP(xp);
    }
}
