using UnityEngine;

public class Collectible : MonoBehaviour
{
    public InstanceManager instanceManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.SendMessage("OnCollected");
            instanceManager.ReleaseAsset(gameObject);
        }
    }
}
