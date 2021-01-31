using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MovingObjectController
{
    private Vector2 _acceleration;
    public float speed = 5f;
    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas menuCanvas;

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
        menuCanvas.gameObject.GetComponent<MenuManager>().canOpen = false;
        winCanvas.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var coll = FindObjectOfType<Collectible>();
            if (coll != null)
                Addressables.ReleaseInstance(coll.gameObject);
            Time.timeScale = 0;
            menuCanvas.gameObject.GetComponent<MenuManager>().canOpen = false;

            loseCanvas.gameObject.SetActive(true);
        }
    }
}
