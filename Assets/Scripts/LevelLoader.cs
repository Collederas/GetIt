using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelLoader : MonoBehaviour
{
    public AssetReference sceneReference;


    public void LoadScene()
    {
        sceneReference.LoadSceneAsync();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
