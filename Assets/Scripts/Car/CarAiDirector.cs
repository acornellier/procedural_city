using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAiDirector
{
    AdjacencyGraph<Vector2Int> _roadGraph;
    Dictionary<Vector2Int, Road> _roadMap;

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
        var intStart = Vector2Int.RoundToInt(start);
        var intEnd = Vector2Int.RoundToInt(end);

        var roadPath = _roadGraph.Dijkstra(intStart, intEnd);

        var path = new List<Vector2>();

        var road = _roadMap[roadPath[0]];
        var nextRoad = _roadMap[roadPath[1]];
        var direction = roadPath[1] - roadPath[0];

        var currentPoint = Vector2.zero;
        for (var i = 0; i < roadPath.Count; ++i)
        {
            // var nextRoad = roadPath[1];
        }

        return path;
    }
}