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
    
    private void Start()
    {
        var startingPoint = new Vector3Int((int)-levelBoundaries.x / 2, (int)levelBoundaries.y / 2, 0);
        _positionIteratorRef = startingPoint;
        GeneratePerimeter(startingPoint);
        GenerateInternal();
    }

    private bool OutOfLevelBounds(Vector3Int position)
    {
        return position.y < -(levelBoundaries.y/2);
    }

    private bool IsPositionOutOfBounds(Vector3Int position)
    {
        return _positionIteratorRef.y > Mathf.Abs(levelBoundaries.y/2) || _positionIteratorRef.x > Mathf.Abs(levelBoundaries.x/2);
    }

    // Checks that there is enough room around chosen spawn point
    private bool IsSpaced(Vector3Int potentialSpawnPoint)
    {
        
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

    private void MoveIteratorRef(int moveAmountX, ref Vector3Int iterator)
    {
        var newPositionX = iterator.x + moveAmountX;
        var amountBeyondBoundaryX = newPositionX - (levelBoundaries.x / 2);
        
        if (amountBeyondBoundaryX > 0)
        {
            // We are beyond boundary, so we start over from next line
            iterator.y -= 1;
            iterator.x = -(levelBoundaries.x/2) + amountBeyondBoundaryX - 1;
        }
        else
        {
            iterator.x = newPositionX;
        }
    }

    private Vector3Int GetNewSpawnPoint()
    {
        var moveAmountX = Random.Range(spacing, spacing + 3);
        MoveIteratorRef(moveAmountX, ref _positionIteratorRef);

        while(levelTilemap.GetTile(_positionIteratorRef) != null)
            MoveIteratorRef(moveAmountX, ref _positionIteratorRef);
        
        return _positionIteratorRef;
    }

    private void GeneratePerimeter(Vector3Int startingPos)
    {
        var position = startingPos;
        while (!OutOfLevelBounds(position))
        {
            if (Mathf.Abs(position.x) == levelBoundaries.x/2 || Mathf.Abs(position.y) == (levelBoundaries.y/2))
                levelTilemap.SetTile(position, wallTile);
            MoveIteratorRef(1, ref position);
        }
    }
    
    private void GenerateInternal()
    {
        while (!OutOfLevelBounds(_positionIteratorRef))
        {
            var spawnPos = GetNewSpawnPoint();
            if (!OutOfLevelBounds(_positionIteratorRef))
            {
                SpawnTileGroup(spawnPos);
            }
        }
        Debug.Log("Finish at " + _positionIteratorRef);
    }
}
