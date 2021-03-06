using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
   
    public TileBase wallTile;
    public Vector2Int levelBoundaries;
    public int maxTilesInGroup = 5;
    public int spacing = 3;  // How much space in between tiles?
    public Tilemap levelTilemap;
    public Canvas winCanvas;
    public Canvas loseCanvas;

    public Dropper dropper;

    private int _levelReached = 0;
    
    private Vector3Int _positionIteratorRef;
    private List<Vector3Int> g_forbiddenSpawnPoints = new List<Vector3Int>();

    private void Start()
    {
        NewLevel();
    }

    public void RestartGame()
    {
        _levelReached = 0;
        NewLevel();
    }

    public void NewLevel()
    {
        _levelReached++;
        levelTilemap.ClearAllTiles();
        var canvasText = winCanvas.gameObject.GetComponentInChildren<Text>();
        canvasText.text = "You reached level: " + _levelReached;
        loseCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        dropper.Initialize();
        var startingPoint = new Vector3Int((int)-levelBoundaries.x / 2, (int)levelBoundaries.y / 2, 0);
        _positionIteratorRef = startingPoint;
        GeneratePerimeter(startingPoint);
        GenerateInternal();
        GeneratePathfinderGraph();
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
    private bool IsSpaced(Vector3Int previousSpawnPoint, Vector3Int newSpawnPoint, int radius)
    {
        return true;
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

    private List<Vector3Int> GetCoordinatesAroundPoint(Vector3Int point)
    {
        var points = new List<Vector3Int>();
        points.Add(new Vector3Int(point.x - 1, point.y, point.z));
        points.Add(new Vector3Int(point.x + 1, point.y, point.z));
        points.Add(new Vector3Int(point.x - 1, point.y - 1, point.z));
        points.Add(new Vector3Int(point.x, point.y - 1, point.z));
        
        points.Add(new Vector3Int(point.x - 2, point.y, point.z));
        points.Add(new Vector3Int(point.x + 2, point.y, point.z));
        points.Add(new Vector3Int(point.x - 2, point.y - 2, point.z));
        points.Add(new Vector3Int(point.x, point.y - 2, point.z));
        return points;
    }
    
    private void SpawnTileGroup(Vector3Int position)
    {
        var spawnedTiles = 0;
        List<Vector3Int> forbiddenSpawnPoints = new List<Vector3Int>();
        
        while (spawnedTiles < Random.Range(1, maxTilesInGroup))
        {
            var spawnPosRepr= Random.Range(1, 4);
            var spawnPos = SpawnPosRepresentationToCoord(position, spawnPosRepr);

            if (!IsPositionOutOfBounds(spawnPos) && !g_forbiddenSpawnPoints.Contains(spawnPos))
            {
                levelTilemap.SetTile(spawnPos, wallTile);
                forbiddenSpawnPoints.AddRange(GetCoordinatesAroundPoint(spawnPos));
                position = spawnPos;
            }
            
            spawnedTiles++;
        }
        g_forbiddenSpawnPoints = forbiddenSpawnPoints;
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
    }

    private void GeneratePathfinderGraph()
    {
        var gg = AstarPath.active.data.gridGraph;
        Debug.Log(gg);
        int width = levelBoundaries.x;
        int depth = levelBoundaries.y;
        float nodeSize = 1;

        gg.center = new Vector3(0.5f, 0.5f, 0);

        gg.SetDimensions(width, depth, nodeSize);
        StartCoroutine(ScanGraph());
    }

    private IEnumerator ScanGraph()
    {
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.Scan();
    }
    
}
