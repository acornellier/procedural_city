using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : MonoBehaviour
{
    Marker[] _carMarkers;

    AdjacencyGraph<Vector2> _adjacencyGraph;

    void Awake()
    {
        _carMarkers = GetComponentsInChildren<Marker>();

        foreach (var marker in _carMarkers)
        {
            foreach (var neighbor in marker.Neighbors)
            {
                _adjacencyGraph.AddEdge(marker.transform.position, neighbor.transform.position);
            }
        }
    }

    List<Vector2> FindPathToNextRoad(Vector2Int nextRoadPosition)
    {
        var exitMarker = FindExitNearestToRoad(nextRoadPosition);
        return new List<Vector2>();
    }

    Marker FindExitNearestToRoad(Vector2Int roadPosition)
    {
        return _carMarkers
            .Where(marker => marker.IsExit)
            .OrderBy(marker => Vector2.Distance(marker.transform.position, roadPosition))
            .First();
    }
}