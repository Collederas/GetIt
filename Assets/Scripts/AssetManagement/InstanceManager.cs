using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "InstanceManager", menuName = "Managers/InstanceManager")]
public class InstanceManager : ScriptableObject
{
    private List<AsyncOperationHandle> _instantiatedHandles = new List<AsyncOperationHandle>();
    
    public void Clear()
    {
        foreach (var handle in _instantiatedHandles)
        {
            Addressables.ReleaseInstance(handle);
        }
    }

    public void InstantiateAsset(AssetReference asset, Vector2 position)
    {
        asset.InstantiateAsync(position, Quaternion.identity).Completed += handle =>
        {
            _instantiatedHandles.Add(handle);
        };
    }
    
    public void ReleaseAsset(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
        _instantiatedHandles.Clear();
    }
}
