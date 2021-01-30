using UnityEngine;

public abstract class MovingObjectController : MonoBehaviour
{
    public Vector2 Velocity { get; set; }

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
}