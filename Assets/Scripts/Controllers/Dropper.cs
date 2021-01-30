using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;


public class Dropper : MonoBehaviour
{
    public AssetReference drop;
    public InstanceManager instanceManager;
    private float _currentTime;
    private bool _dropped = false;
    
    // Drop object at this time
    private float _dropTime;

    private void Start()
    {
        _currentTime = 0;
        _dropTime = Random.Range(1, 2);
    }

    private void Update()
    {
        if (_dropped) return;
        _currentTime += Time.deltaTime;
        if (!(_currentTime >= _dropTime)) return;
        StartCoroutine(DisableCollection());
        instanceManager.InstantiateAsset(drop, transform.position);
        _dropped = true;
    }

    private IEnumerator DisableCollection()
    {
        var cachedTag = gameObject.tag;
        gameObject.tag = "NoCollect";
        yield return new WaitForSeconds(2f);
        gameObject.tag = cachedTag;
    }
}
