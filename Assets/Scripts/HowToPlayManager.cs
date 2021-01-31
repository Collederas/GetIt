using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void EnableHowTo(bool enable)
    {
        gameObject.SetActive(enable);
    }
}

