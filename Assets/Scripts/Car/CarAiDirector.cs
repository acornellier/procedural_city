using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAiDirector
{
    readonly AdjacencyGraph<Vector2Int> _roadGraph;
    readonly Dictionary<Vector2Int, Road> _roadMap;

    public CarAiDirector(AdjacencyGraph<Vector2Int> roadGraph, Dictionary<Vector2Int, Road> roadMap)
    {
        _roadGraph = roadGraph;
        _roadMap = roadMap;
    }

    public List<Vector2> GetRandomPath(Vector2 start)
    {
        var end = _roadMap.Keys.ToList()[Random.Range(0, _roadMap.Count - 1)];
        return GetPath(start, end);
    }

    List<Vector2> GetPath(Vector2 start, Vector2 end)
    {
        D.Log("GetPath", start, end);
        var intStart = Vector2Int.RoundToInt(start);
        var intEnd = Vector2Int.RoundToInt(end);

        var roadPath = _roadGraph.Dijkstra(intStart, intEnd);

        var startMarker = _roadMap[roadPath[0]].FindEntranceNearestToPoint(start);
        var path = new List<Marker> { startMarker, };

        for (var i = 0; i < roadPath.Count - 1; ++i)
        {
            var road = _roadMap[roadPath[i]];
            var roadMarkerPath = road.FindPathToNextRoad(
                path[^1].transform.position,
                roadPath[i + 1]
            );
            path.AddRange(roadMarkerPath);
        }

        return path.Select(marker => (Vector2)marker.transform.position).ToList();
    }
}