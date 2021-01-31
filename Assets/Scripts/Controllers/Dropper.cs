using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;


public class Dropper : MonoBehaviour
{
    public float maxDropTime = 10f;
    
    public AssetReference drop;
    public InstanceManager instanceManager;
    private float _currentTime;
    
    // Drop object at this time
    private float _dropTime;
    private bool _dropped = false;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _dropped = false;
        _currentTime = 0;
        _dropTime = Random.Range(5, maxDropTime);
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
        var thisObj = gameObject;
        var cachedTag = gameObject.tag;
        thisObj.tag = "NoCollect";
        yield return new WaitForSeconds(2f);
        thisObj.tag = cachedTag;
    }
}
