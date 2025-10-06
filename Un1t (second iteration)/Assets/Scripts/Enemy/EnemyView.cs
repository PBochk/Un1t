using UnityEngine;

//Dummy class
//Perfectly, it would be another abstract class in analogy to Controller
//TODO: Stats display / animations logic
public class EnemyView : MonoBehaviour

{
    public Animator animator { get; private set; }
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
