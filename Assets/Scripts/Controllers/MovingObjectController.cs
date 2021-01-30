using UnityEngine;

public abstract class MovingObjectController : MonoBehaviour
{
    public float maxSpeed = 2f;
    public Vector2 Velocity { get; set; }

    protected virtual void Update()
    {
        Velocity = Vector2.ClampMagnitude(Velocity, maxSpeed);
    }

    protected virtual void FixedUpdate()
    {
        var delta = Velocity * Time.deltaTime;
        PerformMovement(delta);
    }

    protected void PerformMovement(Vector2 delta)
    {
        // Transform is only a Vector3 so build delta as a 3D vector. 
        transform.position += new Vector3(delta.x, delta.y, 0);
    }

    public virtual void Fall(Vector2 fallTargetPosition)
    {
    }
}