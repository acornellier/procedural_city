using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        var exitMarker = FindExitNearestToPoint(nextRoadPosition);

        var entranceMarkers = EntrancesNearestToPoint(start);

        foreach (var entranceMarker in entranceMarkers)
        {
            if (entranceMarker && !entranceMarker.Neighbors.Any()) return new[] { entranceMarker, };

            var path = _adjacencyGraph.Dijkstra(entranceMarker, exitMarker);
            if (path.Count > 0)
                return path;
        }

        throw new Exception($"No path found from {start} to {nextRoadPosition}");
    }

    [ItemCanBeNull]
    public List<Marker> EntrancesNearestToPoint(Vector2 point)
    {
        return _carMarkers
            .Where(marker => marker.isEntrance)
            .OrderBy(marker => Vector2.Distance(marker.transform.position, point))
            .ToList();
    }

    Marker FindExitNearestToPoint(Vector2 point)
    {
        return _carMarkers
            .Where(marker => marker.IsExit)
            .OrderBy(marker => Vector2.Distance(marker.transform.position, point))
            .First();
    }
}