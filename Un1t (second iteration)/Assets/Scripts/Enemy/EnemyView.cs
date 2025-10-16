using UnityEngine;

//TODO: Stats display / animations logic
//TODO: figure out what is needed for EnemyView class
//TODO: View should receive Events from the state component, states should not have reference to animator
/// <summary>
/// One of the two MonoBehaviour scripts for enemies, intended to have UI display,
/// animations and sounds logic and data
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/>, <see cref="EnemyState"/> and its derivatives
/// </remarks>
public abstract class EnemyView : MonoBehaviour

{
    private void Awake()
    {
        BindModel();
        BindStates();
        BindSoundPlayer();
        BindAnimator();
    }


    protected abstract void BindModel();
    protected abstract void BindStates();
    protected abstract void BindAnimator();
    protected abstract void BindSoundPlayer();
}
