using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private void OnEnable()
    {
        var text = GetComponent<Text>();
        text.text = "Your Personal Best is Level: " + (PlayerPrefs.GetInt("HighScore") + 1);
    }
}
