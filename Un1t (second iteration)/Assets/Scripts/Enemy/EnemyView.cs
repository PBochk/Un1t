using UnityEngine;

/// <summary>
/// One of the two MonoBehaviour scripts for enemies, intended to have UI display,
/// animations and sounds logic and data
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/>, <see cref="EnemyState"/> and its derivatives
/// </remarks>
[RequireComponent(typeof(EnemyModelMB))]
public abstract class EnemyView : MonoBehaviour
{
    protected EnemyModelMB model;
    
    private void Awake()
    {
        BindModel();
        BindStates();
        BindSoundPlayer();
        BindAnimator();
    }


    protected virtual void BindModel()
    {
        model = GetComponent<EnemyModelMB>();
    }
    protected abstract void BindStates();
    protected abstract void BindAnimator();
    protected abstract void BindSoundPlayer();
}
