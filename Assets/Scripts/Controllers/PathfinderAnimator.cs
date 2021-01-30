using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIPath))]
public class PathfinderAnimator : MonoBehaviour
{
    private AIPath AIPath;
    public Animator _animator;

    void Start()
    {
        AIPath = GetComponent<AIPath>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(AIPath.velocity.magnitude);
        _animator.SetFloat("Speed", AIPath.velocity.magnitude);
    }
}
