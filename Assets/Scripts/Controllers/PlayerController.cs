using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MovingObjectController
{
    private Vector2 _acceleration;
    public float speed = 5f;
    public Canvas winCanvas;
    public Canvas loseCanvas;

    public InstanceManager instanceManager;

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
        Time.timeScale = 0;
        winCanvas.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            instanceManager.Clear();
            Time.timeScale = 0;
            loseCanvas.gameObject.SetActive(true);
        }
    }
}
