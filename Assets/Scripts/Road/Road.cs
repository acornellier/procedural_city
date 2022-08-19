using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : MonoBehaviour
{
    Marker[] _carMarkers;

    AdjacencyGraph<Marker> _adjacencyGraph = new();

    void Awake()
    {
        _carMarkers = GetComponentsInChildren<Marker>();

        foreach (var marker in _carMarkers)
        {
            _adjacencyGraph.AddVertex(marker);
            foreach (var neighbor in marker.Neighbors)
            {
                _adjacencyGraph.AddEdge(marker, neighbor);
            }
        }
    }

    public IEnumerable<Marker> FindPathToNextRoad(Vector2 start, Vector2Int nextRoadPosition)
    {
        var entranceMarker = FindEntranceNearestToPoint(start);
        var exitMarker = FindExitNearestToPoint(nextRoadPosition);
        D.Log("Dijkstra", gameObject.name, entranceMarker, exitMarker);
        return _adjacencyGraph.Dijkstra(entranceMarker, exitMarker);
    }

    public Marker FindEntranceNearestToPoint(Vector2 point)
    {
        return _carMarkers
            .Where(marker => marker.isEntrance)
            .OrderBy(marker => Vector2.Distance(marker.transform.position, point))
            .First();
    }

    Marker FindExitNearestToPoint(Vector2 point)
    {
        return _carMarkers
            .Where(marker => marker.IsExit)
            .OrderBy(marker => Vector2.Distance(marker.transform.position, point))
            .First();
    }
}