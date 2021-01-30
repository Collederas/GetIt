using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AIPath))]
public class MonsterController : MonoBehaviour
{
    private AIPath _pathFinder;
    private GraphNode _currentNode;
    
    private void Start()
    {
        _pathFinder = GetComponent<AIPath>();
    }

    private Vector2 PickRandomPoint () 
    {
        GraphNode randomNode;
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        
        while (!randomNode.Walkable)
            randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];

        var dest = (Vector3)randomNode.position;
        dest.z = 0;
        return dest;
    }
    
    private void Update()
    {
        if (_pathFinder.pathPending || (!_pathFinder.reachedEndOfPath && _pathFinder.hasPath)) return;
        _pathFinder.destination = PickRandomPoint();
    }

    public void OnCollected()
    {
        Debug.Log("You Lose");
    }
}
