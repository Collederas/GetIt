using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
    public TileBase wallTile;
    public Vector2Int levelBoundaries;
    public int maxTilesInGroup = 5;
    public int spacing = 3;  // How much space in between tiles?
    public Tilemap levelTilemap;

    private Vector3Int _positionIteratorRef;
    public InputAction fireAction;

    private void Awake()
    {
        fireAction.performed += Fire;
    }

    void OnEnable()
    {
        fireAction.Enable();
    }

    void Fire(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pos " +  _positionIteratorRef + " is occupied: " + (levelTilemap.GetTile(_positionIteratorRef) != null));
        Debug.Log("We are out of bounds? --> "  + OutOfLevelBounds());

        MoveIteratorRef();
    }

    private void Start()
    {
        var startingPoint = new Vector3Int((int)-levelBoundaries.x / 2, (int)levelBoundaries.y / 2, 0);
        levelTilemap.SetTile(startingPoint, wallTile);
        _positionIteratorRef = startingPoint;
        Generate();
    }

    private bool OutOfLevelBounds()
    {
        return _positionIteratorRef.y < -(levelBoundaries.y/2);
    }

    private void MoveIteratorRef()
    {
        var moveAmountX = Random.Range(spacing, spacing + 2);
        Debug.Log("Move by " + moveAmountX);

        var newPositionX = _positionIteratorRef.x + moveAmountX;
        var amountBeyondBoundaryX = newPositionX - (levelBoundaries.x / 2);
        if (amountBeyondBoundaryX > 0)
        {
            // We are beyond boundary, so we start over from next line
            _positionIteratorRef.y -= 1;
            _positionIteratorRef.x = -(levelBoundaries.x/2) + amountBeyondBoundaryX;
        }
        else
        {
            _positionIteratorRef.x = newPositionX;
        }
    }

    private Vector3Int GetNewSpawnPoint()
    {
        while (levelTilemap.GetTile(_positionIteratorRef) != null)
        {
            MoveIteratorRef();
        }

        return _positionIteratorRef;
    }

    private void Generate()
    {
        while (!OutOfLevelBounds())
        {
            var spawnPoint = GetNewSpawnPoint();
            Debug.Log("Spawn at " + spawnPoint);
        }
        
        Debug.Log("Finish at " + _positionIteratorRef);
    }
}
