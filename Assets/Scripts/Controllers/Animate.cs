using UnityEngine;

[RequireComponent(typeof(MovingObjectController))]
[RequireComponent(typeof(Animator))]
public class Animate : MonoBehaviour
{
    private Animator animator;
    private MovingObjectController movingObject;
    private Vector2 velocityNormalized;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movingObject = GetComponent<MovingObjectController>();
    }

    private void Update()
    {
        velocityNormalized = movingObject.Velocity.normalized;

        var vertical = Vector2.Dot(velocityNormalized, Vector2.up);
        var horizontal = Vector2.Dot(velocityNormalized, Vector2.right);
        var speed = movingObject.Velocity.magnitude / Time.deltaTime;
        Debug.Log(movingObject.Velocity.magnitude);
        // if ((horizontal != 0) | (vertical != 0))
        // {
        //     animator.SetFloat("Horizontal", horizontal);
        //     animator.SetFloat("Vertical", vertical);
        // }

        animator.SetFloat("Speed", speed);
    }
}