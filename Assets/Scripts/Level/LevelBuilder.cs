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
        // Debug.Log("Pos " +  _positionIteratorRef + " is occupied: " + (levelTilemap.GetTile(_positionIteratorRef) != null));
        // Debug.Log("We are out of bounds? --> "  + OutOfLevelBounds());

        Generate();
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

    private bool IsPositionOutOfBounds(Vector3Int position)
    {
        return _positionIteratorRef.y > Mathf.Abs(levelBoundaries.y/2) || _positionIteratorRef.x > Mathf.Abs(levelBoundaries.x/2);
    }

    private Vector3Int SpawnPosRepresentationToCoord(Vector3Int position, int spawnPosIndicator)
    {
        return spawnPosIndicator switch
        {
            1 => new Vector3Int(position.x + 1, position.y, position.z),
            2 => new Vector3Int(position.x, position.y + 1, position.z),
            3 => new Vector3Int(position.x - 1, position.y, position.z),
            4 => new Vector3Int(position.x, position.y - 1, position.z),
            _ => Vector3Int.zero
        };
    }
    
    private void SpawnTileGroup(Vector3Int position)
    {
        var spawnedTiles = 0;
        while (spawnedTiles < Random.Range(1, maxTilesInGroup))
        {
            var spawnPosRepr= Random.Range(1, 4);
            var spawnPos = SpawnPosRepresentationToCoord(position, spawnPosRepr);
            
            if (!IsPositionOutOfBounds(spawnPos))
                levelTilemap.SetTile(spawnPos, wallTile);
            
            spawnedTiles++;
        }
    }

    private void MoveIteratorRef()
    {
        var moveAmountX = Random.Range(spacing, spacing + 3);
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
        MoveIteratorRef();

        while(levelTilemap.GetTile(_positionIteratorRef) != null)
            MoveIteratorRef();
        
        return _positionIteratorRef;
    }

    private void Generate()
    {
        while (!OutOfLevelBounds())
        {
            var spawnPos = GetNewSpawnPoint();
            if (!OutOfLevelBounds())
            {
                SpawnTileGroup(spawnPos);
            }
        }
        Debug.Log("Finish at " + _positionIteratorRef);
    }
}
