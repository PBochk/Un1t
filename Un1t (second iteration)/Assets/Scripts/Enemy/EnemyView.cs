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
public class EnemyView : MonoBehaviour

{
    public Animator animator { get; private set; }
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
