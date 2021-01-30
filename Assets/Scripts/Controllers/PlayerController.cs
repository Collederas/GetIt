using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MovingObjectController
{
    private Vector2 _acceleration;
    public float speed = 5f;
    public void OnMove(InputValue value)
    {
        _acceleration = value.Get<Vector2>();
    }

    public void Update()
    {
        Velocity = _acceleration * speed;
    }

    public void OnCollected()
    {
        Debug.Log("You win :)");
    }
}
